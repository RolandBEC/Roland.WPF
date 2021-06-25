namespace Roland.WPF.Controls.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter (mostly for XAML) that convert a boolean value to his opposite value
    /// </summary>
    public sealed class InvertBooleanConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Return the opposite value of given boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueBool = (bool)value;
            return !valueBool;
        }

        /// <summary>
        /// Return the opposite value of given boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueBool = (bool)value;
            return !valueBool;
        }
        #endregion
    }
}
