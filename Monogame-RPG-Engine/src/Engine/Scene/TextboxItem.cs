using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Scene
{
    public class TextboxItem
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public ScriptActionOutputManager outputManager;

        public TextboxItem(string text)
        {
            Text = text;
        }

        public TextboxItem(string text, List<string> options)
        {
            Text = text;
            Options = options;
        }

        public void AddOption(string option)
        {
            if (Options == null)
            {
                Options = new List<string>();
            }
            Options.Add(option);
        }
    }
}
