using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Roland.WPF.Controls
{
    /// <summary>
    ///     Custom RepeatButton that manage a CornerRadius property
    /// </summary>
    public class RoundedRepeatButton : RepeatButton
    {
        /// <summary>
        ///     Static constructor
        /// </summary>
        static RoundedRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedRepeatButton), new FrameworkPropertyMetadata(typeof(RoundedRepeatButton)));
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
                "CornerRadius", typeof(CornerRadius), typeof(RoundedRepeatButton),
                new FrameworkPropertyMetadata(new CornerRadius(), new PropertyChangedCallback(OnCornerRadiusChanged)));

        private static void OnCornerRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RoundedRepeatButton control = (RoundedRepeatButton)obj;

            RoutedPropertyChangedEventArgs<CornerRadius> e = new RoutedPropertyChangedEventArgs<CornerRadius>(
                (CornerRadius)args.OldValue, (CornerRadius)args.NewValue, CornerRadiusChangedEvent);
            control.OnCornerRadiusChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent CornerRadiusChangedEvent = EventManager.RegisterRoutedEvent(
            "CornerRadiusChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<CornerRadius>), typeof(RoundedRepeatButton));

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
