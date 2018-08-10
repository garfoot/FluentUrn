using System;
using System.Linq;

namespace Garfoot.Utilities.FluentUrn
{
    public class SectionedNamespaceSpecificString : NamespaceSpecificString
    {
        public SectionedNamespaceSpecificString(params string[] sections)
        {
            this.EscapedValue =
                string.Join(":",
                    sections.Select(i =>
                    {
                        i = Escape(i);
                        if (!IsValid(i))
                        {
                            throw new ArgumentException("One or more sections are not a valid section format", nameof(sections));
                        }

                        return i;
                    }));
        }

    }
}