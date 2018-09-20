using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Garfoot.Utilities.FluentUrn
{
    public abstract class NamespaceSpecificString
    {
        private readonly Regex contentRegex = new Regex("([A-Za-z0-9()+,-.:=@;$_!*']|%[a-fA-F0-9]{2})+");

        private string escapedValue;
        /// <summary>
        ///     The un-escaped value of the NSS, this can be used for display purposes or for extracting
        ///     the original data out of the NSS.
        /// </summary>>
        public string UnEscapedValue
        {
            get;
            private set;
        }

        /// <summary>
        ///     Get the escaped value of the NSS, use this for exporting and serialization as it is RFC2141 compliant.
        /// </summary>
        public string EscapedValue
        {
            get => this.escapedValue;
            protected set
            {
                this.escapedValue = value;
                this.UnEscapedValue = UnEscape(value);
            }
        }

        private string UnEscape(string value)
        {
            var builder = new StringBuilder(value.Length);

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '%':
                        if (i + 2 >= value.Length)
                        {
                            throw new ArgumentException("value is not a valid escape sequence", nameof(value));
                        }

                        int charCode = int.Parse(value.Substring(i + 1, 2), NumberStyles.HexNumber);
                        builder.Append((char)charCode);
                        i += 2;

                        break;

                    default:
                        builder.Append(value[i]);
                        break;
                }
            }

            return builder.ToString();
        }

        protected bool IsValid(string content) => this.contentRegex.Match(content).Value == content;

        protected string Escape(string content) => content.Replace("%%", Symbols.Percent)
                                                          .Replace("/", Symbols.Slash)
                                                          .Replace("?", Symbols.Question)
                                                          .Replace("#", Symbols.Octothorpe)
                                                          .Replace(" ", Symbols.Space);

        /// <summary>
        ///     Returns the <see cref="UnEscapedValue" /> of the NSS. To get the escaped value use <see cref="EscapedValue" />.
        /// </summary>
        /// <returns>The unescaped value of the URN.</returns>
        public override string ToString() => this.UnEscapedValue;

        private static class Symbols
        {
            public const string Percent = "%25";
            public const string Slash = "%2F";
            public const string Question = "%3F";
            public const string Octothorpe = "%23";
            public const string Space = "%20";
        }
    }
}