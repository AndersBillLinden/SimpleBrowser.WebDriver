using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SimpleBrowser.Parser;
using SimpleBrowser.WebDriver;

namespace DriverTest
{
	[TestFixture]
	public class XElementExtension
	{
		[Test]
		public void TestInnerHtmlMixingTextAndSubnodes()
		{
			var element = Parse("<div><img>a</div>");
			Assert.AreEqual("<img>a", element.GetInnerHtml());
		}

		[Test]
		public void TestOuterHtmlOnVoidElement()
		{
			var element = Parse("<div><img/></div>");
			Assert.AreEqual("<div><img></div>", element.OuterHtml());
		}

		[Test]
		public void TestOuterHtmlOnNonVoidElement()
		{
			var element = Parse("<div><div/></div>");
			Assert.AreEqual("<div><div></div></div>", element.OuterHtml());
		}

		[Test]
		public void TestInnerHtmlDecapitalization()
		{
			var element = Parse("<div><iMg></div>");
			Assert.AreEqual("<img>", element.GetInnerHtml());
		}

		[Test]
		public void TestInnerHtmlAttribute()
		{
			var element = Parse("<div><img src=\"foo\"></div>");
			Assert.AreEqual("<img src=\"foo\">", element.GetInnerHtml());
		}

		[Test]
		public void TestInnerHtmlAttributeWithApostrophes()
		{
			var element = Parse("<div><img src='foo'></div>");
			Assert.AreEqual("<img src=\"foo\">", element.GetInnerHtml());
		}

		[Test]
		public void TestOuterHtmlAttributeWithEscapeableCharacters()
		{
			var element = Parse("<img src='foo'>");
			element.SetAttributeValue("src","a\"b");

			Assert.AreEqual("<img src=\"a&quot;b\">", element.OuterHtml());
		}

		[Test]
		public void TestSetInnerHtml()
		{
			var element = Parse("<div><img></div>");
			element.SetInnerHtml("<select id='test'>");
			Assert.AreEqual("<div><select id=\"test\"></select></div>", element.OuterHtml());
		}

		private XElement Parse(string html)
		{
			return html.ParseHtml().Root.Elements().First();
		}
	}
}
