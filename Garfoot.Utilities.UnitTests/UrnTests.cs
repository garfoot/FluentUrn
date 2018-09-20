using System;
using FluentAssertions;
using Garfoot.Utilities.FluentUrn;
using NUnit.Framework;

namespace Garfoot.Utilities.UnitTests
{
    [TestFixture]
    [Category("Unit")]
    public class UrnTests
    {
        [TestCase("valid", "valid")]
        [TestCase("valid/", "valid%2F")]
        [TestCase("valid%%", "valid%25")]
        [TestCase("valid?", "valid%3F")]
        [TestCase("valid#", "valid%23")]
        public void Ctor_ValidUrnWithEscaping_ReturnsOk(string content, string expected)
        {
            var urn = new Urn("anyNid", new RawNamespaceSpecificString(content));

            urn.EscapedValue.Should().Be($"urn:anyNid:{expected}");
        }

        [TestCase("urn")]
        [TestCase("_wrong")]
        [TestCase("wro_ng")]
        public void Ctor_InvalidNamespaceId_Throws(string namespaceId)
        {
            Assert.Throws<ArgumentException>(() =>
                new Urn(namespaceId, new SectionedNamespaceSpecificString("anySection1", "anySection2")));
        }

        [TestCase("%zy")]
        [TestCase("this`is`invalid")]
        [TestCase(@"\wrong")]
        public void Ctor_InvalidNamespaceSpecificString_Throws(string namespaceSpecificString)
        {
            Assert.Throws<ArgumentException>(() =>
                new Urn("any-namespace", new RawNamespaceSpecificString(namespaceSpecificString)));
        }

        [TestCase("urn:test-id:some%2Fvalue%2Fhere", "test-id", "some/value/here")]
        public void Parse_ValidUrns_ReturnsUrn(string content, string namespaceSpecificString, string expectedContent)
        {
            Urn urn = Urn.Parse(content);

            urn.NamespaceIdentifier.Should().Be(namespaceSpecificString);
            urn.GetContent<RawNamespaceSpecificString>().ToString().Should().Be(expectedContent);
        }

        [Test]
        public void FluentBuild_SectionedUrn_ReturnsUrn()
        {
            Urn urn = UrnBuilder.CreateUrn("any-namespace")
                .WithSectionedContent("item1", "item2")
                .Build();

            urn.EscapedValue.Should().BeEquivalentTo("urn:any-namespace:item1:item2");
        }

        [Test]
        public void FluentBuild_RawUrn_ReturnsUrn()
        {
            Urn urn = UrnBuilder.CreateUrn("any-namespace")
                .WithRawContent("item1:item2")
                .Build();

            urn.EscapedValue.Should().BeEquivalentTo("urn:any-namespace:item1:item2");
        }

        [Test]
        public void Parse_GenericNss_ReturnsUrn()
        {
            Urn urn = Urn.Parse<SectionedNamespaceSpecificString>("urn:anyId:section1:section2");

            urn.UnEscapedValue.Should().Be("urn:anyId:section1:section2");
        }
    }
}