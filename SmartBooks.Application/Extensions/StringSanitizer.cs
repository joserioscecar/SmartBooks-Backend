using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace SmartBooks.Application.Extensions
{
    public static class StringSanitizer
    {
        public static string WithoutDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveAccents(this string texto)
        {
            var acentos = new Dictionary<char, char>
        {
            { 'á', 'a' }, { 'é', 'e' }, { 'í', 'i' }, { 'ó', 'o' }, { 'ú', 'u' },{ 'Á', 'A' }, { 'É', 'E' }, { 'Í', 'I' }, { 'Ó', 'O' }, { 'Ú', 'U' }
        };

            var sb = new StringBuilder();
            foreach (char c in texto)
            {

                if (acentos.ContainsKey(c))
                {
                    sb.Append(acentos[c]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
