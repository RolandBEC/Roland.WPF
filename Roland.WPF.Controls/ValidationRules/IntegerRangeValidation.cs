namespace Roland.WPF.Controls.ValidationRules
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    /// <summary>
    ///     Validation for integer input
    ///     The return <see cref="ValidationResult"/> is false if the text in not a integer or not between <see cref="Min"/> and <see cref="Max"/> value
    /// </summary>
    public class IntegerRangeValidation : ValidationRule
    {
        #region Constants, Fields, Properties, Indexers

        #region Properties, Indexers
        public int Max { get; set; }
        public int Min { get; set; }
        #endregion

        #endregion

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public IntegerRangeValidation(int minValue, int maxValue)
        {
            this.Min = minValue;
            this.Max = maxValue;
        }

        #region Overrides of ValidationRule
        /// <summary>
        ///     When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check</param>
        /// <param name="cultureInfo">The culture to use in this rule</param>
        /// <returns>A System.Windows.Controls.ValidationResult object.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _errorMsg = string.Format(
                CultureInfo.CurrentCulture,
                "Value must be an integer between {0} and {1}.",
                this.Min,
                this.Max);

            try
            {
                string _sValue = value?.ToString();
                int _valueToValidate;
                if (_sValue?.Length <= 0 || !int.TryParse(_sValue, out _valueToValidate))
                {
                    return new ValidationResult(false, _errorMsg);
                }

                if ((_valueToValidate < this.Min) || (_valueToValidate > this.Max))
                {
                    return new ValidationResult(false, _errorMsg);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, _errorMsg);
            }

            return ValidationResult.ValidResult;
        }
        #endregion
    }
}
