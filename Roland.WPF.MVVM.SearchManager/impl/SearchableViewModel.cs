using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Roland.WPF.MVVM.SearchManager.impl
{
    public abstract class SearchableViewModel : ISearchableViewModel
    {
        public List<MatchedProperty> MatchedProperties { get; } = new List<MatchedProperty>();

        public int MatchNumber { get; protected set; }

        private List<PropertyToSearch> pPropertiesToSearch;
        public IReadOnlyList<PropertyToSearch> PropertiesToSearch { get; }

        private SearchManagerMode pSearchMode;

        protected SearchableViewModel()
        {
            this.pPropertiesToSearch = new List<PropertyToSearch>();
            this.PropertiesToSearch = new ReadOnlyCollection<PropertyToSearch>(this.pPropertiesToSearch);
        }

        /// <summary>
        ///     Check if the <see cref="ISearchableViewModel"/> contains a property that match with <param name="textToSearch"></param> among those who are in <see cref="PropertiesToSearch"/>
        ///     This method is also supposed to fill <see cref="PropertiesToSearch"/> before doing the search
        /// </summary>
        /// <param name="textToSearch"></param>
        /// <param name="stringComparison"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool IsViewModelMatch(string textToSearch, StringComparison stringComparison, SearchManagerMode mode)
        {
            this.pSearchMode = mode;
            this.ReinitPropertiesToSearch();
            this.MatchNumber = 0;
            if (string.IsNullOrEmpty(textToSearch))
            {
                return false;
            }

            bool _result = false;
            foreach(PropertyToSearch _property in this.PropertiesToSearch.Where(x => x.PropertyValue != null))
            {
                RegexOptions _regOpt = 
                    stringComparison == StringComparison.OrdinalIgnoreCase
                    || stringComparison  == StringComparison.CurrentCultureIgnoreCase 
                    || stringComparison  == StringComparison.InvariantCultureIgnoreCase 
                    ? RegexOptions.IgnoreCase : RegexOptions.None;

                int _matches = Regex.Matches(_property.PropertyValue.ToString(), $@"{Regex.Escape(textToSearch)}", _regOpt).Count;
                if(_matches > 0)
                {
                    this.MatchNumber += _matches;
                    this.MatchedProperties.Add(new MatchedProperty(_property, _matches));
                    _result = true;
                }

            }
            return _result;
        }

        public abstract bool ReplaceText(MatchedProperty prop, string textToReplace, string newText, StringComparison stringComparison, int iteration);

        protected void AddPropertyToSearch(string propertyName, object propertyValue, bool isAssociatedControlVisible)
        {
            if (isAssociatedControlVisible || (this.pSearchMode == SearchManagerMode.REPLACE_MODE && !this.CanPropertyValueBeReplaced(propertyName)))
            {
                return;
            }
            this.pPropertiesToSearch.Add(new PropertyToSearch(propertyName, propertyValue));
        }


        protected void AddPropertyToSearch(string propertyName, object propertyValue)
        {
            this.AddPropertyToSearch(propertyName, propertyValue, true);
        }

        protected abstract bool CanPropertyValueBeReplaced(string propertyName);

        protected void ClearPropertiesToSearchList()
        {
            this.pPropertiesToSearch.Clear();
            this.MatchedProperties.Clear();
        }

        /// <summary>
        ///     This method should be used to fill <see cref="pPropertiesToSearch"/>
        ///     Fill it in the order that the search should do
        /// </summary>
        protected abstract void ReinitPropertiesToSearch();
    }
}
