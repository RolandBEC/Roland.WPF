using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    public interface ISearchableViewModelContainer<T> where T : ISearchableViewModel
    {
    }
}
