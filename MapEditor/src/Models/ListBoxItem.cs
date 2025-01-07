using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Models
{
    public class ListBoxItem<T>
    {
        public T Value { get; set; }
        public string Display { get; set; }

        public ListBoxItem(T value, string display)
        {
            Value = value;
            Display = display;
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
