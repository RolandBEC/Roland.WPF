using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Roland.WPF.Controls
{
    /// <summary>
    ///     Custom button that manage a CornerRadius property
    /// </summary>
    public class RoundedButton : Button
    {
        /// <summary>
        ///     Static constructor
        /// </summary>
        static RoundedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedButton), new FrameworkPropertyMetadata(typeof(RoundedButton)));
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
                "CornerRadius", typeof(CornerRadius), typeof(RoundedButton),
                new FrameworkPropertyMetadata(new CornerRadius(), new PropertyChangedCallback(OnCornerRadiusChanged)));

        private static void OnCornerRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RoundedButton control = (RoundedButton)obj;

            RoutedPropertyChangedEventArgs<CornerRadius> e = new RoutedPropertyChangedEventArgs<CornerRadius>(
                (CornerRadius)args.OldValue, (CornerRadius)args.NewValue, CornerRadiusChangedEvent);
            control.OnCornerRadiusChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent CornerRadiusChangedEvent = EventManager.RegisterRoutedEvent(
            "CornerRadiusChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<CornerRadius>), typeof(RoundedButton));

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
