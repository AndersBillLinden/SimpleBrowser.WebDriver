using System.Linq;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class HtmlWebElement
	{
		private IWebElement _source;
		public HtmlWebElement(IWebElement source)
		{
			_source = source;
		}

		public HtmlWebElement appendChild(HtmlWebElement child)
		{
			_source.AsHtmlElement().Element.Add(child._source.AsHtmlElement().Element);

			return child;
		}

		public HtmlWebElement getElementById(string id)
		{
			var element = _source.FindElement(By.Id(id));

			return new HtmlWebElement(element);
		}

		public HtmlWebElement[] getElementsByClassName(string classname)
		{
			return _source.FindElements
				(By.ClassName(classname)).Select(element => new HtmlWebElement(element)).ToArray();
		}

		public HtmlWebElement[] getElementsByTagName(string tagname)
		{
			return _source.FindElements
				(By.TagName(tagname)).Select(element => new HtmlWebElement(element)).ToArray();
		}

		public string id
		{
			get { return _source.AsHtmlElement().GetAttributeValue("id"); }
		}

		public string className
		{
			get { return _source.AsHtmlElement().GetAttributeValue("class"); }
		}

		public string innerHTML
		{
			get
			{
				return _source.AsHtmlElement().Element.GetInnerHtml();
			}

			set
			{
				_source.AsHtmlElement().Element.SetInnerHtml(value);
			}
		}

		public string outerHTML
		{
			get
			{
				return _source.AsHtmlElement().Element.OuterHtml();
			}
		}
	}
}
