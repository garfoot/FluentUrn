using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Garfoot.Utilities.FluentUrn
{
    public class SectionedNamespaceSpecificString : NamespaceSpecificString, INamespaceSpecificStringParser
    {
        public IReadOnlyList<string> Sections { get; private set; }

        /// <summary>
        ///     Default constructor. Use the <see cref="Parse"/> method to initialize this instance.
        /// </summary>
        public SectionedNamespaceSpecificString()
        {
        }

        /// <summary>
        ///     Create and initialize an instance.
        /// </summary>
        /// <param name="sections">The sections to initialize with.</param>
        public SectionedNamespaceSpecificString(params string[] sections)
        {
            InitializeSections(sections);
        }

        /// <summary>
        ///     Parse the content into the current namespace specific string instance.
        /// </summary>
        /// <param name="content">The content to parse.</param>
        void INamespaceSpecificStringParser.Parse(string content)
        {
            InitializeSections(content.Split(':'));
        }

        /// <summary>
        ///     Parse the content into a new namespace specific string instance.
        /// </summary>
        /// <param name="content">The content to parse.</param>
        public static SectionedNamespaceSpecificString Parse(string content)
        {
            var nss = new SectionedNamespaceSpecificString();
            ((INamespaceSpecificStringParser)nss).Parse(content);
            return nss;
        }

        private void InitializeSections(string[] sections)
        {
            this.Sections = new ReadOnlyCollection<string>(sections);

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