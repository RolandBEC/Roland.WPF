using Roland.WPF.SearchManager.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager.impl
{
    public abstract class SearchableViewModelContainer : ISearchableViewModelContainer<ISearchableViewModel>
    {
        private StringComparison pStringComparison;
        private int pMatchIndex;
        private ISearchableViewModel pSearchableViewModel;
        private string pTextToHighlight;
        private ISearchableView pView;

        public ISearchBehavior LastBehavior { get; set; }
        public ISearchableView View 
        { 
            get
            {
                return this.pView;
            }
            set 
            { 
                if(this.pView == null)
                {
                    return;
                }
                this.View = value;
                if(this.pView != null && this.pTextToHighlight != null)
                {
                    this.HighlightMatch(this.pSearchableViewModel, this.pTextToHighlight, this.pStringComparison, this.pMatchIndex);
                    this.pTextToHighlight = null;
                    this.pSearchableViewModel = null;
                    this.pMatchIndex = 0;
                }
            }
        }

        public virtual ISearchableViewModel GetCurrentSearchableViewModel()
        {
            return this.GetSearchableViewModels?.First();
        }

        public abstract IEnumerable<ISearchableViewModel> GetSearchableViewModels();

        public void HighlightMatch(ISearchableViewModel matchedViewModel, string textToHighlight, StringComparison stringComparison, int index)
        {
            FindMatchProperty(matchedViewModel, index, (property, localIndex) =>
                this.Highlight(matchedViewModel, property, textToHighlight, stringComparison, index, localIndex));
        }

        public int ReplaceAllMatches(string oldText, string newText, StringComparison stringComparison)
        {
            RegexOptions _regOpt =
                    stringComparison == StringComparison.OrdinalIgnoreCase
                    || stringComparison == StringComparison.CurrentCultureIgnoreCase
                    || stringComparison == StringComparison.InvariantCultureIgnoreCase
                    ? RegexOptions.IgnoreCase : RegexOptions.None;

            int _failedReplace = 0;
            foreach(ISearchableViewModel _searchableViewModel in this.GetSearchableViewModels())
            {
                foreach(MatchedProperty _matchedProp in _searchableViewModel.MatchedProperties)
                {
                    bool _result = _searchableViewModel.ReplaceText(_matchedProp, oldText, newText, stringComparison, -1);
                    if (_result)
                    {
                        continue;
                    }

                    string _propValue = _matchedProp.PropertyValue.ToString();
                    string _pattern = @"(" + oldText + @").*?";
                    MatchCollection _regexResult = Regex.Matches(_propValue, _pattern, _regOpt);
                    if(_regexResult.Count > 0)
                    {
                        _failedReplace += _regexResult.Count;
                    }
                    else
                    {
                        _failedReplace++;
                    }
                }
            }
            return _failedReplace;
        }

        public bool ReplaceMatch(ISearchableViewModel matchedViewModel, string oldText, string newText, StringComparison stringComparison, int index)
        {
            bool _isMatchReplaced = false;
            FindMatchProperty(matchedViewModel, index, (property, localIndex)
                => _isMatchReplaced = matchedViewModel.ReplaceText(property, oldText, newText, stringComparison, index - localIndex));
            return _isMatchReplaced;
        }

        public void UnHighlightMatch()
        {
            this.LastBehavior?.UnHighlightText();
            this.LastBehavior = null;
        }

        protected abstract void FocusISearchableViewModelForHighlight(ISearchableViewModel searchableViewModel);

        protected abstract void Highlight(ISearchableViewModel searchableViewModel, MatchedProperty matched, string textToHighlight, StringComparison stringComparison, int index, int localIndex);

        /// <summary>
        ///     Find matched property and execute code related to the found <see cref="MatchedProperty"/> and the index of the text to search
        /// </summary>
        /// <param name="matcheViewModel"></param>
        /// <param name="index"></param>
        /// <param name="Exec"></param>
        public static void FindMatchProperty(ISearchableViewModel matcheViewModel, int index, Action<MatchedProperty, int> Exec)
        {
            int pLocalIndex = 0;
            foreach(MatchedProperty _property in matcheViewModel.MatchedProperties)
            {
                if(index > pLocalIndex + (_property.MatchNumber - 1))
                {
                    pLocalIndex += _property.MatchNumber;
                    continue;
                }
                Exec(_property, pLocalIndex);
                return;
            }
        }
    }
}
