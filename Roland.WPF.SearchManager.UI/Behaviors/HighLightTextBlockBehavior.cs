using Roland.WPF.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Roland.WPF.SearchManager.UI.Behaviors
{
    public class HighLightTextBlockBehavior : AbstractHighLightBehavior<TextBlock>
    {
        public override void HighlightText(string textToSearch, StringComparison stringComparison, int index)
        {
            this.ItemsPositions.Clear();
            this.UpdateListOfMatchedItem(textToSearch, stringComparison);

            if(!this.ItemsPositions.Any() || index < 0 || index >= this.ItemsPositions.Count)
            {
                return;
            }

            this.LastIndexOfItemsPositionList = index;
            TextRange _tr = this.ItemsPositions[index];
            _tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Yellow);
            _tr.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Black);

            this.AssociatedObject.BringIntoView();
        }

        public override void UnHighlightText()
        {
            throw new NotImplementedException();
        }

        protected override void ReinitHighlightItem(StringComparison stringComparison)
        {
            this.AssociatedObject.Foreground = Brushes.Black;
            this.AssociatedObject.Background = this.AssociatedObject.Background;

            bool _canGetWordRange = false;
            switch (stringComparison)
            {
                case StringComparison.CurrentCulture:
                case StringComparison.Ordinal:
                case StringComparison.InvariantCulture:
                    _canGetWordRange = this.AssociatedObject.Text.Contains(this.TextToHighlight);
                    break;
                case StringComparison.InvariantCultureIgnoreCase:
                case StringComparison.OrdinalIgnoreCase:
                    _canGetWordRange = this.AssociatedObject.Text.ToLower(CultureInfo.InvariantCulture).Contains(this.TextToHighlight.ToLower(CultureInfo.InvariantCulture));
                    break;
                case StringComparison.CurrentCultureIgnoreCase:
                    _canGetWordRange = this.AssociatedObject.Text.ToLower(CultureInfo.CurrentCulture).Contains(this.TextToHighlight.ToLower(CultureInfo.CurrentCulture));
                    break;
                default:
                    // DO NOTHING
                    break;
            }

            if (_canGetWordRange)
            {
                this.ItemsPositions.Clear();
                this.ItemsPositions.AddRange(this.GetAllWordRange(this.TextToHighlight, stringComparison));
            }
        }

        private List<TextRange> GetAllWordRange(string textToSearch, StringComparison stringComparison)
        {
            return TextRangeUtils.FindTextInRange(this.AssociatedObject.ContentStart, this.AssociatedObject.ContentEnd, textToSearch, stringComparison);
        }
    }
}
