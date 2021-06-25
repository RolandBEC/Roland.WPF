using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager
{
    public interface ISearchManager
    {

        /// <summary>
        ///     The property leads the way how the search will be done for the case
        ///     if we search by ignoring the case or not
        /// </summary>
        bool IsMatchCase { get; set; }

        /// <summary>
        ///     The property leads the way how the seatch will be done for the case
        ///     if we search on property that can be modified or not
        /// </summary>
        bool IsReplaceMode { get; set; }

        /// <summary>
        ///     Searched text value
        /// </summary>
        string SearchedText { get; }

        /// <summary>
        ///     Count all matches found
        /// </summary>
        /// <returns></returns>
        int CountAllMatches();

        /// <summary>
        ///     Replace all found matches by the given <param name="textToReplace"></param>
        /// </summary>
        /// <param name="textToReplace"></param>
        /// <returns></returns>
        int ReplaceAllMatches(string textToReplace);

        /// <summary>
        ///     Replace the current highlighted match by the given <param name="textToReplace"></param>
        /// </summary>
        /// <param name="textToReplace"></param>
        /// <returns></returns>
        int ReplaceCurrentMatch(string textToReplace);

        /// <summary>
        ///     Unhighight the re-highlight the current match
        /// </summary>
        void ResetMatch();

        /// <summary>
        ///     Search and highlight next match
        /// </summary>
        /// <returns></returns>
        bool SearchNextMatch();

        /// <summary>
        ///     Search and highlight previous match
        /// </summary>
        /// <returns></returns>
        bool SearchPreviousMatch();

        /// <summary>
        ///     Set the text to search and update the matches number
        ///     NOTE : The function does not focus or highlight
        /// </summary>
        /// <param name="text"></param>
        /// <param name="mode"></param>
        void SetSearchText(string text, SearchManagerMode mode);

        /// <summary>
        ///     Verify that we already have a match
        /// </summary>
        /// <returns></returns>
        bool HasMatch();
    }
}
