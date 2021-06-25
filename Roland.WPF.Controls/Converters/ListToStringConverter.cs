namespace Roland.WPF.Controls.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Converter (mostly for XAML) that convert a list of string to a string
    /// </summary>
    public sealed class ListToStringConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Converter a list of string to a string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> valueList = (List<string>)value;
            string parameterString = parameter as string;

            if (parameterString?.ToLower(CultureInfo.CurrentCulture) == "newlineseparator")
            {
                return valueList == null ? string.Empty : string.Join("\n", valueList);
            }
            return valueList == null ? string.Empty : string.Join(",", valueList);
        }

        /// <summary>
        /// Return a list of string by given string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            string valueString = (string)value;

            string parameterString = parameter as string;

            return parameterString?.ToLower(CultureInfo.CurrentCulture) == "newlineseparator"
                ? valueString.Split('\n').ToList()
                : valueString.Split(',').ToList();
        }
        #endregion
    }
}
