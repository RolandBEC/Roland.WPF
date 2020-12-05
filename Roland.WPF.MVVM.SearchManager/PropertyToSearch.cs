using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    /// <summary>
    ///     Simple data class used to store property to search with her value
    /// </summary>
    public class PropertyToSearch
    {
        /// <summary>
        ///     Name of the property to search on
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        ///     Value of the property to search on
        /// </summary>
        public object PropertyValue { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public PropertyToSearch(string propertyName, object propertyValue)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="prop"></param>
        public PropertyToSearch(PropertyToSearch prop)
        {
            if (prop == null) return;
            this.PropertyName = prop.PropertyName;
            this.PropertyValue = prop.PropertyValue;
        }
    }
}
