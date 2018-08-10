using System;

namespace Garfoot.Utilities.FluentUrn
{
    public class RawNamespaceSpecificString : NamespaceSpecificString
    {
        public RawNamespaceSpecificString(string content)
        {
            content = Escape(content);

            if (!IsValid(content))
            {
                throw new ArgumentException("Specified content is not in a valid format", nameof(content));
            }

            this.EscapedValue = content;
        }
    }
}