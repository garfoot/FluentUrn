namespace Garfoot.Utilities.FluentUrn
{
    public interface IUrnIdentifier
    {
        /// <summary>
        ///     Add raw content to the URN, this will be escaped.
        /// </summary>
        /// <param name="content">The content to add.</param>
        /// <returns></returns>
        IRawUrn WithRawContent(string content);

        /// <summary>
        ///     Add sections of content to the URN separated by ':'. Each section will
        ///     be individually escaped.
        /// </summary>
        /// <param name="content">The content sections to add.</param>
        /// <returns></returns>
        ISectionedContent WithSectionedContent(params string[] content);

        /// <summary>
        ///     Add a custom namespace specific string to the URN.
        /// </summary>
        /// <typeparam name="TNss">The type of NSS to add.</typeparam>
        /// <param name="content">The NSS to add that contains the content for the URN.</param>
        /// <returns></returns>
        IBuildableUrn WithCustomNss<TNss>(TNss content)
            where TNss : NamespaceSpecificString;
    }

    public interface IRawUrn : IBuildableUrn
    {
    }

    public interface ISectionedContent : IBuildableUrn
    {
    }

    public interface IBuildableUrn
    {
        /// <summary>
        ///     Build the <see cref="Urn"/>.
        /// </summary>
        /// <returns>A new URN object.</returns>
        Urn Build();
    }
}