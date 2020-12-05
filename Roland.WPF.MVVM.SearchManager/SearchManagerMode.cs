using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    /// <summary>
    ///     Lead the SearchManager's behavior
    /// </summary>
    public enum SearchManagerMode
    {
        // Will search on all searchable property
        SEARCH_MODE, 
        // Will on search property that are editable
        REPLACE_MODE
    }
}
