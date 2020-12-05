using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Roland.WPF.SearchManager.UI.Behaviors
{
    public abstract class AbstractHighLightBehavior<T> : Behavior<T>, ISearchBehavior where T : FrameworkElement
    {
        protected int LastIndexOfItemsPositionList;
        protected List<TextRange> ItemsPositions { get; } = new List<TextRange>();
        protected string TextToHighlight;

        public abstract void HighlightText(string textToSearch, StringComparison stringComparison, int index);

        public abstract void UnHighlightText();

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
            this.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
            this.AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            this.UnHighlightText();
        }

        private void AssociatedObject_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            this.UnHighlightText();
        }

        protected void UpdateListOfMatchedItem(string textToSearch, StringComparison stringComparison)
        {
            if(textToSearch != null)
            {
                this.TextToHighlight = textToSearch;
            }

            if(this.AssociatedObject == null || !this.AssociatedObject.IsVisible || this.TextToHighlight == null)
            {
                return;
            }

            this.ReinitHighlightItem(stringComparison);
        }

        protected abstract void ReinitHighlightItem(StringComparison stringComparison);
    }
}
