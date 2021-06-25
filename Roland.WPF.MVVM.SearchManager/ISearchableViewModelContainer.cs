using Roland.WPF.SearchManager.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    /// <summary>
    ///     Class that implement this interface have to be the viewmodel that manage the view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearchableViewModelContainer<T> where T : ISearchableViewModel
    {

        ISearchBehavior LastBehavior { get; set; }

        ISearchableView View { get; set; }

        /// <summary>
        ///     Return the current select <see cref="ISearchableViewModel"/>
        /// </summary>
        /// <returns></returns>
        T GetCurrentSearchableViewModel();

        /// <summary>
        ///     Return the list of all <see cref="ISearchableViewModel"/> contained in the <see cref="ISearchableViewModelContainer"/>
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetSearchableViewModels();

        /// <summary>
        ///     Highlight the match of the <see cref="ISearchableViewModelContainer"/>
        /// </summary>
        /// <param name="matchedViewModel"></param>
        /// <param name="textToHighlight"></param>
        /// <param name="stringComparison"></param>
        /// <param name="index"></param>
        void HighlightMatch(T matchedViewModel, string textToHighlight, StringComparison stringComparison, int index);

        /// <summary>
        ///     Replace all the matches contains in the <see cref="ISearchableViewModelContainer"/>
        /// </summary>
        /// <param name="oldText"></param>
        /// <param name="newText"></param>
        /// <param name="stringComparison"></param>
        /// <returns>Return the number of unreplaced match</returns>
        int ReplaceAllMatches(string oldText, string newText, StringComparison stringComparison);

        /// <summary>
        ///     Replace text in the given match
        /// </summary>
        /// <param name="matchedViewModel"></param>
        /// <param name="oldText"></param>
        /// <param name="newText"></param>
        /// <param name="stringComparison"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        bool ReplaceMatch(T matchedViewModel, string oldText, string newText, StringComparison stringComparison, int index);

        void UnHighlightMatch();
    }
}
