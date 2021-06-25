using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Roland.WPF.Controls
{
    /// <summary>
    ///     Overwrite of the slider control
    ///     Manage zoom in and out buttons that modify the <see cref="Slider.Value" /> by incrementing with
    ///     <see cref="Slider.TickFrequency" />
    ///     Also manage a textBlock that display the percentage
    /// </summary>
    [TemplatePart(Name = "PART_ZoomInButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_ZoomOutButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_ZoomInfo", Type = typeof(TextBlock))]
    public class ZoomSlider : Slider
    {
        #region Constants, Fields, Properties, Indexers

        #region Fields, Constants
        /// <summary>
        ///     Identifies the BaseZoomValue dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseZoomValueProperty = DependencyProperty.Register(
            "BaseZoomValue",
            typeof(double),
            typeof(ZoomSlider),
            new FrameworkPropertyMetadata(1.0d, BaseZoomValuePropertyChangedCallback),
            IsValidDoubleValue);

        /// <summary>
        ///     Identifies the DefaultZoomValue dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultZoomValueProperty = DependencyProperty.Register(
            "DefaultZoomValue",
            typeof(double),
            typeof(ZoomSlider),
            new FrameworkPropertyMetadata(1.0d),
            IsValidDoubleValue);

        private TextBlock pZoomInfo;
        #endregion

        #region Properties, Indexers
        /// <summary>
        ///     Base value on which the calculus percentage of zoom will be based
        ///     The default value is 1
        /// </summary>
        public double BaseZoomValue
        {
            get
            {
                return (double)this.GetValue(BaseZoomValueProperty);
            }
            set
            {
                this.SetValue(BaseZoomValueProperty, value);
            }
        }

        /// <summary>
        ///     Default value of the zoom slider
        ///     This property is used when the <see cref="SetDefaultValue"/> command is called
        /// </summary>
        public double DefaultZoomValue
        {
            get
            {
                return (double)this.GetValue(DefaultZoomValueProperty);
            }
            set
            {
                this.SetValue(DefaultZoomValueProperty, value);
            }
        }

        /// <summary>
        ///     Reset the <see cref="Slider.Value"/> property
        /// </summary>
        public static RoutedCommand SetDefaultValue { get; private set; }
        #endregion

        #endregion

        /// <summary>
        ///     Static constructor
        /// </summary>
        static ZoomSlider()
        {
            // Initialize CommandCollection & CommandLink(s)
            InitializeCommands();

            // Register all PropertyTypeMetadata
            MinimumProperty.OverrideMetadata(
                typeof(ZoomSlider),
                new FrameworkPropertyMetadata(0.1d, FrameworkPropertyMetadataOptions.AffectsMeasure));
            MaximumProperty.OverrideMetadata(
                typeof(ZoomSlider),
                new FrameworkPropertyMetadata(1.9d, FrameworkPropertyMetadataOptions.AffectsMeasure));
            ValueProperty.OverrideMetadata(
                typeof(ZoomSlider),
                new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsMeasure));

            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ZoomSlider),
                new FrameworkPropertyMetadata(typeof(ZoomSlider)));
        }

        #region Overrides of Slider
        /// <summary>
        ///     Apply the template associated to ZoomSlider
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Apply the template of parent class (Slider)
            base.OnApplyTemplate();

            // Retrieve buttons and textblock from the template
            this.pZoomInfo = this.GetTemplateChild("PART_ZoomInfo") as TextBlock;
            if (this.pZoomInfo != null)
            {
                // I set a default displayed value. Not the best way to do it.
                this.pZoomInfo.Text = "100%";
            }
        }
        #endregion

        #region Overrides of Slider
        /// <summary>
        ///     Call when Slider.DecreaseSmall command is invoked.
        ///     Override the default behavior that only remove the <see cref="Slider.TickFrequency" /> to the
        ///     <see cref="Slider.Value" />
        ///     Now it move to the previous available number that match with the <see cref="Slider.TickFrequency" />
        /// </summary>
        protected override void OnDecreaseSmall()
        {
            double _roundedValue = Math.Round(this.Value, 2, MidpointRounding.AwayFromZero);
            double _modulo = _roundedValue % this.TickFrequency;
            double _roundedModulo = Math.Round(_modulo, 2, MidpointRounding.AwayFromZero);
            if (_roundedModulo.CompareTo(0) <= 0 || _roundedModulo.CompareTo(this.TickFrequency) == 0)
            {
                this.Value -= this.TickFrequency;
                return;
            }
            this.Value = _roundedValue - _modulo;
        }
        #endregion

        #region Overrides of Slider
        /// <summary>
        ///     Call when Slider.IncreaseSmall command is invoked.
        ///     Override the default behavior that only add the <see cref="Slider.TickFrequency" /> to the
        ///     <see cref="Slider.Value" />
        ///     Now it move to the next available number that match with the <see cref="Slider.TickFrequency" />
        /// </summary>
        protected override void OnIncreaseSmall()
        {
            double _roundedValue = Math.Round(this.Value, 2, MidpointRounding.AwayFromZero);
            double _modulo = _roundedValue % this.TickFrequency;
            double _roundedModulo = Math.Round(_modulo, 2, MidpointRounding.AwayFromZero);
            if (_roundedModulo.CompareTo(0) <= 0 || _roundedModulo.CompareTo(this.TickFrequency) == 0)
            {
                this.Value = _roundedValue + this.TickFrequency;
                return;
            }
            this.Value = _roundedValue + (this.TickFrequency - _modulo);
        }
        #endregion

        #region Overrides of Slider
        /// <summary>
        ///     Listen the change of the value property in order to display the percentage
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            this.UpdateDisplayedPercentage();
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Allow to move to next available tick
        ///     NOTE : I'm not really happy with this implementation. I would prefer to use some bindable command or event handler
        ///     instead of using a public method
        /// </summary>
        public void MoveToNextTick()
        {
            this.OnIncreaseSmall();
        }

        /// <summary>
        ///     Allow to move to previous available tick
        ///     NOTE : I'm not really happy with this implementation. I would prefer to use some bindable command or event handler
        ///     instead of using a public method
        /// </summary>
        public void MoveToPreviousTick()
        {
            this.OnDecreaseSmall();
        }

        /// <summary>
        ///     Callback when the  <see cref="BaseZoomValue" /> property change
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dependencyPropertyChangedEventArgs"></param>
        private static void BaseZoomValuePropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ZoomSlider _zoomSlider = dependencyObject as ZoomSlider;
            _zoomSlider?.UpdateDisplayedPercentage();
        }

        /// <summary>
        ///     Initialize command related to the ZoomSlider control
        /// </summary>
        static void InitializeCommands()
        {
            SetDefaultValue = new RoutedCommand(nameof(SetDefaultValue), typeof(ZoomSlider));
            CommandManager.RegisterClassCommandBinding(
                typeof(ZoomSlider),
                new CommandBinding(SetDefaultValue, OnSetDefaultValueCommand, null));
        }

        /// <summary>
        ///     Validate input value in RangeBase (Minimum, Maximum, and Value).
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns False if value is NaN or NegativeInfinity or PositiveInfinity. Otherwise, returns True.</returns>
        private static bool IsValidDoubleValue(object value)
        {
            double d = (double)value;

            return !(double.IsNaN(d) || double.IsInfinity(d));
        }

        /// <summary>
        ///     Execute the SetBaseValue command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="executedRoutedEventArgs"></param>
        private static void OnSetDefaultValueCommand(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            ZoomSlider slider = sender as ZoomSlider;
            if (slider != null)
            {
                slider.Value = slider.DefaultZoomValue;
            }
        }

        /// <summary>
        ///     Update the display of the zoom info
        /// </summary>
        private void UpdateDisplayedPercentage()
        {
            if (this.pZoomInfo == null)
            {
                return;
            }
            if (this.BaseZoomValue.CompareTo(0) == 0)
            {
                this.pZoomInfo.Text = "Invalid base zoom !!!";
                return;
            }
            double _percentage = this.Value * 100 / this.BaseZoomValue;
            this.pZoomInfo.Text = string.Format(CultureInfo.CurrentCulture, "{0}%", Math.Round(_percentage));
        }
        #endregion
    }
}
