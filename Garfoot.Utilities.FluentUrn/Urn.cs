using System;
using System.Text.RegularExpressions;

namespace Garfoot.Utilities.FluentUrn
{
    public class Urn
    {
        private static readonly Regex NamespaceIdRegex = new Regex("^[a-zA-Z0-9][a-zA-Z0-9\\-]{1,31}$");

        public string NamespaceIdentifier
        {
            get;
        }

        /// <summary>
        ///     Get the escaped value of the URN, use this for exporting and serialization as it is RFC2141 compliant.
        /// </summary>
        public string EscapedValue
        {
            get;
        }

        /// <summary>
        ///     The un-escaped value of the URN, this can be used for display purposes or for extracting
        ///     the original data out of the URN.
        /// </summary>
        public string UnEscapedValue
        {
            get;
        }

        private readonly NamespaceSpecificString content;

        /// <summary>
        ///     Create a new URN.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier.</param>
        /// <param name="content">A <see cref="NamespaceSpecificString"/>.</param>
        public Urn(string namespaceIdentifier, NamespaceSpecificString content)
        {
            if (!IsValidNamespaceId(namespaceIdentifier))
            {
                throw new ArgumentException(nameof(namespaceIdentifier), "Namespace identifier is not valid");
            }

            this.content = content;

            this.NamespaceIdentifier = namespaceIdentifier;
            this.UnEscapedValue = $"urn:{this.NamespaceIdentifier}:{GetContent<NamespaceSpecificString>()}";
            this.EscapedValue = $"urn:{this.NamespaceIdentifier}:{GetContent<NamespaceSpecificString>()?.EscapedValue}";
        }

        /// <summary>
        ///     Parse a new URN from a given source.
        /// </summary>
        /// <typeparam name="TNss">The type of namespace specific string (NSS) to parse.</typeparam>
        /// <param name="source">The source URN.</param>
        /// <returns>A new <see cref="Urn"/> instance.</returns>
        public static Urn Parse<TNss>(string source)
            where TNss : NamespaceSpecificString, INamespaceSpecificStringParser, new()
        {
            return Parse(source, content =>
            {
                var instance = Activator.CreateInstance<TNss>();
                instance.Parse(content);
                return instance;
            });
        }

        /// <summary>
        ///     Parse a URN from a given source. Call into an NSS factory to control parsing of the
        ///     namespace specific string.
        /// </summary>
        /// <param name="source">The source URN.</param>
        /// <param name="nssFactory">
        ///     A factory method that will create and parse into a namespace specific
        ///     string (NSS). Default is null which will use a <see cref="RawNamespaceSpecificString"/>.
        /// </param>
        /// <returns></returns>
        public static Urn Parse(string source, Func<string, NamespaceSpecificString> nssFactory = null)
        {
            string[] strings = source.Split(new[] {':'}, 3);
            if (strings.Length < 3)
            {
                throw new ArgumentException("Source string is not a valid URN. Too few sections", nameof(source));
            }

            if (!string.Equals("urn", strings[0], StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Source string is not a valid URN. Does not start with 'urn:'");
            }

            if (!IsValidNamespaceId(strings[1]))
            {
                throw new ArgumentException("Source string is not a valid URN. Namespace ID is invalid");
            }

            nssFactory = nssFactory ?? (content => new RawNamespaceSpecificString(content));

            return new Urn(strings[1], nssFactory(strings[2]));
        }
        

        /// <summary>
        ///     Get the content of the URN in a specific format.
        /// </summary>
        /// <typeparam name="T">The format to attempt to get from the URN.</typeparam>
        /// <returns>An object of type T or null if the object is not of that type.</returns>
        public T GetContent<T>()
            where T : NamespaceSpecificString
        {
            return this.content as T;
        }

        /// <summary>
        ///     Check if the namespace id is valid.
        /// </summary>
        /// <param name="namespaceId">The namespace Id to check.</param>
        /// <returns>True if valid, false if not.</returns>
        private static bool IsValidNamespaceId(string namespaceId) => NamespaceIdRegex.Match(namespaceId).Value == namespaceId
                                                               && !string.Equals(namespaceId, "urn", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Returns the unescaped value of the URN.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.UnEscapedValue;
    }
}