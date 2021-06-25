namespace Roland.WPF.Controls.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Data;

    /// <summary>
    /// /// Converter (mostly for XAML) that convert a enum value value to a her description
    /// </summary>
    public sealed class EnumDescriptionConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Return a description attribute by given an enum value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            Enum myEnum = (Enum)value;
            string description = this.GetEnumDescription(myEnum);
            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return the description attribute of given enum
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
            return attrib?.Description;
        }
        #endregion
    }
}
