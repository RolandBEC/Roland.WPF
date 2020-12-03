using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Roland.WPF.Controls
{
    /// <summary>
    ///     Custom ToggleButton that manage a CornerRadius property
    /// </summary>
    public class RoundedToggleButton : ToggleButton
    {
        /// <summary>
        ///     Static constructor
        /// </summary>
        static RoundedToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedToggleButton), new FrameworkPropertyMetadata(typeof(RoundedToggleButton)));
        }

        #region CornerRadius property 
        /// <summary>
        /// 
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Identifies the CornerRadius dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius", typeof(CornerRadius), typeof(RoundedToggleButton),
                new FrameworkPropertyMetadata(new CornerRadius(), new PropertyChangedCallback(OnCornerRadiusChanged)));

        private static void OnCornerRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RoundedToggleButton control = (RoundedToggleButton)obj;

            RoutedPropertyChangedEventArgs<CornerRadius> e = new RoutedPropertyChangedEventArgs<CornerRadius>(
                (CornerRadius)args.OldValue, (CornerRadius)args.NewValue, CornerRadiusChangedEvent);
            control.OnCornerRadiusChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent CornerRadiusChangedEvent = EventManager.RegisterRoutedEvent(
            "CornerRadiusChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<CornerRadius>), typeof(RoundedToggleButton));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<CornerRadius> CornerRadiusChanged
        {
            add { AddHandler(CornerRadiusChangedEvent, value); }
            remove { RemoveHandler(CornerRadiusChangedEvent, value); }
        }
        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnCornerRadiusChanged(RoutedPropertyChangedEventArgs<CornerRadius> args)
        {
            RaiseEvent(args);
        }
        #endregion

    }
}
