using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Roland.WPF.Controls.Converters
{
    /// <summary>
    /// Converter (mostly for XAML) that convert a boolean value to a visibility value
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Return a visibility value depending of the given boolean and parameter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = (bool)value;
            string parameterString = parameter as string;
            Visibility result;
            switch (parameterString)
            {
                case "thenVisibleElseHidden":
                    result = (bValue) ? Visibility.Visible : Visibility.Hidden;
                    break;
                case "reversed":
                    result = (bValue) ? Visibility.Collapsed : Visibility.Visible;
                    break;
                default:
                    result = (bValue) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Return a boolean value depending of the given visibility value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
