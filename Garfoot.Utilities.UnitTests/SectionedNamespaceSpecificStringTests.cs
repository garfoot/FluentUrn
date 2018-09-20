using FluentAssertions;
using Garfoot.Utilities.FluentUrn;
using NUnit.Framework;

namespace Garfoot.Utilities.UnitTests
{
    [TestFixture]
    [Category("Unit")]
    public class SectionedNamespaceSpecificStringTests
    {
        [Test]
        public void Parse_ValidItem_ReturnsSections()
        {
            var expected = new[]
            {
                "any",
                "valid",
                "sections"
            };

            var result = SectionedNamespaceSpecificString.Parse("any:valid:sections");
            result.Sections.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Ctor_SectionsWithSpaces_ReturnsNew()
        {
            var expected = new[]
            {
                "An",
                "Item with spaces"
            };

            var nss = new SectionedNamespaceSpecificString("An", "Item with spaces");

            nss.Sections.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Ctor_SectionsWithSpaces_EscapesCorrectly()
        {
            var expected = "An:Item%20with%20spaces";

            var nss = new SectionedNamespaceSpecificString("An", "Item with spaces");

            nss.EscapedValue.Should().Be(expected);
        }
    }
}