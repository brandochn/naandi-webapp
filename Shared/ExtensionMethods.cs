using System;
using System.Collections.Generic;
using System.Text;

namespace Naandi.Shared
{
    public static class ExtensionMethods
    {
        public static string ConvertJsonSpecialCharactersToAscii(this string characters)
        {
            if (characters == null)
                return string.Empty;

            string result = characters.Replace("\\u00c0", "À", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c1", "Á", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c2", "Â", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c3", "Ã", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c4", "Ä", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c5", "Å", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c6", "Æ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c7", "Ç", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c8", "È", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00c9", "É", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ca", "Ê", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00cb", "Ë", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00cc", "Ì", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00cd", "Í", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ce", "Î", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00cf", "Ï", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d1", "Ñ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d2", "Ò", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d3", "Ó", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d4", "Ô", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d5", "Õ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d6", "Ö", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d8", "Ø", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00d9", "Ù", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00da", "Ú", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00db", "Û", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00dc", "Ü", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00dd", "Ý", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00df", "ß", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e0", "à", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e1", "á", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e2", "â", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e3", "ã", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e4", "ä", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e5", "å", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e6", "æ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e7", "ç", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e8", "è", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00e9", "é", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ea", "ê", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00eb", "ë", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ec", "ì", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ed", "í", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ee", "î", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ef", "ï", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f0", "ð", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f1", "ñ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f2", "ò", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f3", "ó", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f4", "ô", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f5", "õ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f6", "ö", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f8", "ø", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00f9", "ù", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00fa", "ú", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00fb", "û", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00fc", "ü", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00fd", "ý", StringComparison.OrdinalIgnoreCase)
                .Replace("\\u00ff", "ÿ", StringComparison.OrdinalIgnoreCase)
                .Replace("\\r", "")
                .Replace("\\n", "");

            return result;
        }
    }
}
