using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Roland.WPF.MVVM.Common
{
    /// <summary>
    ///     Class use for XAML it allow us to easily keep a data
    /// </summary>
    public class BindingProxy : Freezable
    {
        #region Constants, Fields, Properties, Indexers

        #region Fields, Constants
        /// <summary>
        ///     Declaration of DataProperty
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(object),
            typeof(BindingProxy),
            new UIPropertyMetadata(null));
        #endregion

        #region Properties, Indexers
        /// <summary>
        ///     Declaration of Data
        /// </summary>
        public object Data
        {
            get
            {
                return this.GetValue(DataProperty);
            }
            set
            {
                this.SetValue(DataProperty, value);
            }
        }
        #endregion

        #endregion

        #region Overrides of Freezable
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
        #endregion
    }
}
