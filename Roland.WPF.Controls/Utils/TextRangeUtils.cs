using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Roland.WPF.C_Sharp_Utils;

namespace Roland.WPF.Controls.Utils
{
    public static class TextRangeUtils
    {
        /// <summary>
        ///     Search the  <param name="searchedText"></param> between <param name="start"></param> and <param name="end"></param>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="searchedText"></param>
        /// <param name="stringComparison"></param>
        /// <returns>Return a list of TextRange that contains the text</returns>
        public static List<TextRange> FindTextInRange(TextPointer start, TextPointer end, string searchedText, StringComparison stringComparison)
        {
            if(start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if(end == null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            if (string.IsNullOrEmpty(searchedText))
            {
                return new List<TextRange>();
            }

            FlowPart[] _flowParts = RetrieveText(start, end).ToArray();
            string wholeText = _flowParts.Aggregate(string.Empty, (_current, part) => _current + part.Text);
            int[] _positions = wholeText.AllIndexesOf(searchedText, stringComparison);
            if(_positions == null || _positions.Length < 1)
            {
                // The searched text is not found
                return new List<TextRange>();
            }

            return (from position in _positions
                    let startPointer = GetStartPointer(_flowParts, position)
                    let endPointer = GetEndPointer(_flowParts, position + searchedText.Length)
                    select new TextRange(startPointer, endPointer)).ToList();

        }

        /// <summary>
        ///     Return the textPointer that match with the offset
        /// </summary>
        /// <param name="flowPartArray"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static TextPointer GetStartPointer(IEnumerable<FlowPart> flowPartArray, int offset)
        {
            int _counter = offset;
            foreach(FlowPart _flowPart in flowPartArray)
            {
                if(_flowPart.Text.Length > _counter)
                {
                    return _flowPart.Tp.GetPositionAtOffset(_counter);
                }
                _counter -= _flowPart.Text.Length;
            }
            // Not supposed to happen
            return null;
        }

        /// <summary>
        ///     Return the textPointer that match with the offset
        /// </summary>
        /// <param name="flowPartArray"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static TextPointer GetEndPointer(IEnumerable<FlowPart> flowPartArray, int offset)
        {
            int _counter = offset;
            foreach(FlowPart _flowPart in flowPartArray)
            {
                if(_flowPart.Text.Length >= _counter)
                {
                    return _flowPart.Tp.GetPositionAtOffset(_counter);
                }
                _counter -= _flowPart.Text.Length;
            }
            // Not supposed to happen
            return null;
        }

        /// <summary>
        ///     Retrieve the displayed text between the given textPointer
        ///     The retrieved text can be decomposed depending the rtf decoration
        ///     The concatenation of the element of the result represent the whole text
        ///     
        ///     DEV NOTE : This is the best solution that you will ever found if you want to access to the display rich text through FlowDocument or TextRange.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static IEnumerable<FlowPart> RetrieveText(TextPointer start, TextPointer end)
        {
            List<FlowPart> _result = new List<FlowPart>();

            int _offsetNb = start.GetOffsetToPosition(end);
            bool _acceptNextText = false;

            for(int i = 0; i < _offsetNb; i++)
            {
                TextPointer _tp = start.GetPositionAtOffset(i);
                if(_tp == null)
                {
                    continue;
                }

                TextPointerContext _textPointerContext = _tp.GetPointerContext(LogicalDirection.Forward);
                if(_textPointerContext == TextPointerContext.ElementStart)
                {
                    // The current _textPointerContext is a TextPointerContext.ElementStart
                    // so we will only accept the next _textPointerContext that will be a TextPointerContext.Text
                    _acceptNextText = true;
                    continue;
                }

                if(_textPointerContext == TextPointerContext.Text && _acceptNextText)
                {
                     string _text = _tp.GetTextInRun(LogicalDirection.Forward);
                    _result.Add(new FlowPart(_tp, _text));

                    // Now that we retrieved the text, we will no more accept TextPointerContext.Text until we encounter a TextPointerContext.ElementStart
                    _acceptNextText = false;
                }
            }

            return _result;
        }

        /// <summary>
        ///     Inner class that associated a testPointer with the text in run locatin within
        /// </summary>
        private class FlowPart
        {
            /// <summary>
            ///     Text from run
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            ///     Textpointer where the run begin
            /// </summary>
            public TextPointer Tp { get; set; }

            public FlowPart(TextPointer tp, string text)
            {
                this.Tp = tp;
                this.Text = text;
            }
        }
    }
}
