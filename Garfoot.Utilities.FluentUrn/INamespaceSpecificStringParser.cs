using System;

namespace Garfoot.Utilities.FluentUrn
{
    /// <summary>
    ///     Implemented by namespace specific strings that support parsing.
    /// </summary>
    public interface INamespaceSpecificStringParser
    {
        /// <summary>
        ///     Parse the content into the current namespace specific string instance.
        /// </summary>
        /// <param name="content">The content to parse.</param>
        void Parse(string content);
    }
}