using Roland.WPF.MVVM.SearchManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.SearchManager.UI
{
    public interface ISearchableView
    {

        ISearchBehavior HighlightCellContent(string textToSearch, StringComparison stringComparison, MatchedProperty matchedProperty, int index);

        ISearchBehavior HighlightMatch(string textToSearch, StringComparison stringComparison, string propertyName, int matchIndex);
    }
}
