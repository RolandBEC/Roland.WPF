using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    /// <summary>
    ///     Simple data class that inherit from <see cref="PropertyToSearch"/> used for search feature
    /// </summary>
    public class MatchedProperty : PropertyToSearch
    {
        /// <summary>
        ///     Number of match(es) found in the associated property value
        /// </summary>
        public int MatchNumber { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="matchNumber"></param>
        public MatchedProperty(PropertyToSearch prop, int matchNumber) : base(prop)
        {
            this.MatchNumber = matchNumber;
        }
    }
}
