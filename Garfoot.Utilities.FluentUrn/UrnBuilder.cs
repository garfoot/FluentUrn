namespace Garfoot.Utilities.FluentUrn
{
    public class UrnBuilder : IUrnIdentifier, IRawUrn, ISectionedContent
    {
        private string identifier;
        private NamespaceSpecificString namespaceSpecificString;

        /// <summary>
        ///     Starts building a new URN.
        /// </summary>
        /// <param name="identifier">The namespace identifier for the URN.</param>
        /// <returns></returns>
        public static IUrnIdentifier CreateUrn(string identifier)
        {
            var builder = new UrnBuilder
            {
                identifier = identifier
            };

            return builder;
        }

        /// <summary>
        ///     Add raw content to the URN, this will be escaped.
        /// </summary>
        /// <param name="content">The content to add.</param>
        /// <returns></returns>
        public IRawUrn WithRawContent(string content)
        {
            this.namespaceSpecificString = new RawNamespaceSpecificString(content);
            return this;
        }

        /// <summary>
        ///     Add sections of content to the URN separated by ':'. Each section will
        ///     be individually escaped.
        /// </summary>
        /// <param name="content">The content sections to add.</param>
        /// <returns></returns>
        public ISectionedContent WithSectionedContent(params string[] content)
        {
            this.namespaceSpecificString = new SectionedNamespaceSpecificString(content);
            return this;
        }

        /// <summary>
        ///     Add a custom namespace specific string to the URN.
        /// </summary>
        /// <typeparam name="TNss">The type of NSS to add.</typeparam>
        /// <param name="content">The NSS to add that contains the content for the URN.</param>
        /// <returns></returns>
        public IBuildableUrn WithCustomNss<TNss>(TNss content)
            where TNss : NamespaceSpecificString
        {
            this.namespaceSpecificString = content;
            return this;
        }

        /// <summary>
        ///     Build the <see cref="Urn"/>.
        /// </summary>
        /// <returns>A new URN object.</returns>
        public Urn Build() => new Urn(this.identifier, this.namespaceSpecificString);
    }
}