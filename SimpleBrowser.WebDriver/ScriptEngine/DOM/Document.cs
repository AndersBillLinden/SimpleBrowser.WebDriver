using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class Document
	{
		private SimpleBrowserDriver _browser;
		public Document(SimpleBrowserDriver browser)
		{
			_browser = browser;
		}

		public HtmlWebElement getElementById(string id)
		{
			var element = _browser.FindElement(By.Id(id));

			return new HtmlWebElement(element);
		}

		public HtmlWebElement[] getElementsByClassName(string classname)
		{
			return _browser.FindElements
				(By.ClassName(classname)).Select(element => new HtmlWebElement(element)).ToArray();
		}

		public HtmlWebElement[] getElementsByTagName(string tagname)
		{
			return _browser.FindElements
				(By.TagName(tagname)).Select(element => new HtmlWebElement(element)).ToArray();
		}

		public HtmlWebElement createElement(string tagName)
		{
			var element = HtmlElement.CreateFor(new XElement(tagName));

			var htmlResult = new HtmlResult(new List<HtmlElement>(){ element }, _browser.Browser);

			var webElement = new WebElement(new HtmlResultWrapper(htmlResult));

			return new HtmlWebElement(webElement);
		}
	}
}
