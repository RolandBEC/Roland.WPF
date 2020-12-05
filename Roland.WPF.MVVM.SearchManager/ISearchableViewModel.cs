using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    /// <summary>
    ///     Class that implement this interface are supposed to be the class that contains displayed data in the view
    /// </summary>
    public interface ISearchableViewModel
    {
        /// <summary>
        ///     This list used to list all matched properties among all that are contained in <see cref="PropertiesToSearch"/>
        /// </summary>
        List<MatchedProperty> MatchedProperties { get; }

        /// <summary>
        ///     Number of match(es) found in the <see cref="ISearchableViewModel"/>
        /// </summary>
        int MatchNumber { get; }

        /// <summary>
        ///     This list is used to list all the properties that we want to search in <see cref="ISearchableViewModel"/>
        /// </summary>
        IReadOnlyList<PropertyToSearch> PropertiesToSearch { get; }

        /// <summary>
        ///     Check if the <see cref="ISearchableViewModel"/> contains a property that match with <param name="textToSearch"></param> among those who are in <see cref="PropertiesToSearch"/>
        ///     This method is also supposed to fill <see cref="PropertiesToSearch"/> before doing the search
        /// </summary>
        /// <param name="textToSearch"></param>
        /// <param name="stringComparison"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool IsViewModelMatch(string textToSearch, StringComparison stringComparison, SearchManagerMode mode);

        bool ReplaceText(MatchedProperty prop, string textToReplace, string newText, StringComparison stringComparison, int iteration);
    }
}
