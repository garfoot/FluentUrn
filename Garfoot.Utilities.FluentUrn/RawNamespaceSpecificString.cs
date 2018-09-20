using System;

namespace Garfoot.Utilities.FluentUrn
{
    public class RawNamespaceSpecificString : NamespaceSpecificString, INamespaceSpecificStringParser
    {
        /// <summary>
        ///     Default constructor. Use the <see cref="Parse"/> method to initialize this instance.
        /// </summary>
        public RawNamespaceSpecificString()
        {
        }

        /// <summary>
        ///     Construct and initialize this instance.
        /// </summary>
        /// <param name="content"></param>
        public RawNamespaceSpecificString(string content)
        {
            content = Escape(content);

            if (!IsValid(content))
            {
                throw new ArgumentException("Specified content is not in a valid format", nameof(content));
            }

            this.Parse(content);
        }

        /// <summary>
        ///     Parse the content into the current namespace specific string instance.
        /// </summary>
        /// <param name="content">The content to parse.</param>
        public void Parse(string content)
        {
            this.EscapedValue = content;
        }
    }
}