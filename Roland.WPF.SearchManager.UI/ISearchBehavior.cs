using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.SearchManager.UI
{
    public interface ISearchBehavior
    {
        void HighlightText(string textToSearch, StringComparison stringComparison, int index);

        void UnHighlightText();
    }
}
