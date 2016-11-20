using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace EditorConfig
{
    [Export(typeof(IIntellisenseControllerProvider))]
    [ContentType(ContentTypes.EditorConfig)]
    [Name("EditorConfig TypeThrough Completion Controller")]
    [Order(Before = "Default Completion Controller")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class MarkdownTypeThroughControllerProvider : IIntellisenseControllerProvider
    {
        public IIntellisenseController TryCreateIntellisenseController(ITextView view, IList<ITextBuffer> subjectBuffers)
        {
            if (subjectBuffers.Count > 0)
            {
                return view.Properties.GetOrCreateSingletonProperty(() => new MarkdownTypeThroughController(view, subjectBuffers));
            }

            return null;
        }
    }

    internal class MarkdownTypeThroughController : TypeThroughController
    {
        public MarkdownTypeThroughController(ITextView textView, IList<ITextBuffer> subjectBuffers)
            : base(textView, subjectBuffers)
        { }

        protected override bool CanComplete(ITextBuffer textBuffer, int position)
        {
            return EditorConfigLanguage.preferences != null ? EditorConfigLanguage.preferences.EnableMatchBraces : false;

            //if (result)
            //{
            //    var line = textBuffer.CurrentSnapshot.GetLineFromPosition(position);
            //    result = line.Start.Position + line.GetText().TrimEnd('\r', '\n', ' ', ';', ',').Length == position + 1;
            //}

            //return result;
        }

        protected override char GetCompletionCharacter(char typedCharacter)
        {
            switch (typedCharacter)
            {
                case '[':
                    return ']';

                case '(':
                    return ')';

                case '{':
                    return '}';
            }

            return '\0';
        }
    }
}

