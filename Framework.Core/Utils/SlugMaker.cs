using System;

namespace EvilDuck.Framework.Core.Utils
{
    public class SlugMaker
    {
        public static string SlugFor<TEntity>(TEntity entity, Func<TEntity, string> propAccessor,
            Func<TEntity, object> parentAccessor = null, string concatenator = "_")
        {
            if (parentAccessor != null)
            {
                var firstPart = parentAccessor(entity).ToString();
                var secondPart = propAccessor(entity);

                return String.Format("{0}{2}{1}", Sanitize(firstPart), Sanitize(secondPart), concatenator);
            }
            else
            {
                var firstPart = propAccessor(entity);
                return Sanitize(firstPart);
            }
        }

        private static string Sanitize(string str)
        {
            return str
                .Replace("!", String.Empty)
                .Replace("@", String.Empty)
                .Replace("#", String.Empty)
                .Replace("$", String.Empty)
                .Replace("%", String.Empty)
                .Replace("^", String.Empty)
                .Replace("*", String.Empty)
                .Replace("(", String.Empty)
                .Replace(")", String.Empty)
                .Replace("+", String.Empty)
                .Replace("=", String.Empty)
                .Replace("{", String.Empty)
                .Replace("[", String.Empty)
                .Replace("}", String.Empty)
                .Replace("]", String.Empty)
                .Replace("|", String.Empty)
                .Replace("\\", String.Empty)
                .Replace(":", String.Empty)
                .Replace(";", String.Empty)
                .Replace("\"", String.Empty)
                .Replace("'", String.Empty)
                .Replace("<", String.Empty)
                .Replace(">", String.Empty)
                .Replace(",", String.Empty)
                .Replace(".", String.Empty)
                .Replace("?", String.Empty)
                .Replace("/", String.Empty)
                .Replace("ę", "e")
                .Replace("Ę", "E")
                .Replace("ó", "o")
                .Replace("Ó", "O")
                .Replace("ą", "a")
                .Replace("Ą", "A")
                .Replace("ś", "s")
                .Replace("Ś", "S")
                .Replace("ł", "l")
                .Replace("Ł", "L")
                .Replace("ż", "z")
                .Replace("Ż", "Z")
                .Replace("ź", "z")
                .Replace("Ź", "Z")
                .Replace("ć", "c")
                .Replace("Ć", "C")
                .Replace("ń", "n")
                .Replace("Ń", "N")
                .Replace(" ", "-")
                .Replace("\t", "-")
                .Replace("\r", "-")
                .Replace("\n", String.Empty);
        }
    }
}