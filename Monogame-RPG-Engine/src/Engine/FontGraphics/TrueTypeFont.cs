using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// just a wrapper around a byte[] true type font file
namespace Monogame_RPG_Engine.Engine.FontGraphics
{
    public class TrueTypeFont
    {
        public byte[] Source { get; }

        public TrueTypeFont(byte[] source)
        {
            Source = source;
        }
    }
}
