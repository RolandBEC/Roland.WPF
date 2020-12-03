using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Roland.WPF.Controls.Controls
{
    /// <summary>
    ///     Custom datagridcolumn that display a DatePicker inside it
    /// </summary>
    public class DataGridDatePickerColumn : DataGridBoundColumn
    {
        /// <summary>
        ///     Display a desired control when the cell is in edit mode
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            DatePicker datepicker = new DatePicker();
            this.ApplyBinding(datepicker, DatePicker.SelectedDateProperty);
            return datepicker;
        }

        /// <summary>
        ///     Display a desired control when the cell in not in edit mode
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            // I display a datePicker, but I could have displayed a textBlock
            DatePicker datepicker = new DatePicker();
            this.ApplyBinding(datepicker, DatePicker.SelectedDateProperty);
            return datepicker;
        }

        /// <summary>
        ///     Apply the <see cref="this.Binding"/> to given <param name="property"></param> of <param name="target"></param>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        private void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            BindingBase binding = this.Binding;
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
                return;
            }
            BindingOperations.ClearBinding(target, property);
        }

    }
}
