using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.ExtensionMethods
{
    public static class SizeExtensions
    {
        public static Size Multiply(this Size size, double factor)
        {
            return new Size((int)(size.Width * factor), (int)(size.Height * factor));
        }
    }
}
