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