using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class TextboxScriptAction : ScriptAction
    {
        protected List<TextboxItem> textboxItems;

        public TextboxScriptAction()
        {
            this.textboxItems = new List<TextboxItem>();
        }

        public TextboxScriptAction(String text)
        {
            this.textboxItems = new List<TextboxItem>();
            this.textboxItems.Add(new TextboxItem(text));
        }

        public TextboxScriptAction(String[] textItems)
        {
            this.textboxItems = new List<TextboxItem>();
            foreach (string text in textItems)
            {
                textboxItems.Add(new TextboxItem(text));
            }
        }

        public TextboxScriptAction(List<string> textItems)
        {
            this.textboxItems = new List<TextboxItem>();
            foreach (string text in textItems)
            {
                textboxItems.Add(new TextboxItem(text));
            }
        }

        public TextboxScriptAction AddText(string text)
        {
            this.textboxItems.Add(new TextboxItem(text));
            return this;
        }

        public TextboxScriptAction AddText(TextboxItem text)
        {
            this.textboxItems.Add(text);
            return this;
        }

        public TextboxScriptAction AddText(string text, String[] options)
        {
            this.textboxItems.Add(new TextboxItem(text, new List<string>(options)));
            return this;
        }

        public override void Setup()
        {
            TextboxItem[] textboxItemsArray = textboxItems.ToArray();
            this.map.Textbox.AddText(textboxItemsArray);
            this.map.Textbox.IsActive = true;
        }

        public override ScriptState Execute()
        {
            if (!this.map.Textbox.IsTextQueueEmpty())
            {
                return ScriptState.RUNNING;
            }
            return ScriptState.COMPLETED;
        }

        public override void Cleanup()
        {
            this.map.Textbox.IsActive = false;
        }
    }
}
