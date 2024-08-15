using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Extensions
{
    public static class StringExtensions
    {
        public static string SubstringByIndexes(this string value, int startIndex, int endIndex)
        {
            return value.Substring(startIndex, endIndex - startIndex);
        }
    }
}
