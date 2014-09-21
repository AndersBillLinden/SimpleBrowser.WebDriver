using System.Collections.Generic;
using DriverTest.Extensions;
using DriverTest.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleBrowser;
using SimpleBrowser.WebDriver;

namespace DriverTest
{
	[TestFixture]
	public class ScriptHost
	{
		[Test]
		public void TestAlert()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/test.html",

					"<script>" +
						"alert('hi');" +
					"</script>"
				}
			}, "http://localhost/test.html");

			Assert.AreEqual("hi", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestConsoleLog()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/test.html",

					"<script>" +
						"console.log('hi');" +
					"</script>"
				}
			}, "http://localhost/test.html");

			Assert.AreEqual("hi", browser.ScriptHost.ConsoleLogs.ToCommaString());
		}

		[Test]
		public void TestBodyOnLoad()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/test.html",

					"<head>" +
						"<script>" +
							"alert('loading');" +
							"function on_body_load()" +
							"{" +
								"alert('loaded');" +
							"}" +
							"alert('loading more');" +
						"</script>" +
					"</head>" +
					"<body onload=\"on_body_load()\">" +
					"</body>"
				}
			}, "http://localhost/test.html");

			Assert.AreEqual("loading,loading more,loaded", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestClickHandler()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/test.html",

					"<head>" +
						"<script>" +
							"function on_click()" +
							"{" +
								"alert('clicked');" +
							"}" +
						"</script>" +
					"</head>" +
					"<body>" +
						"<div id='foo' onclick='on_click()'>click me</div>" +
					"</body>"
				}
			}, "http://localhost/test.html");

			browser.FindElement(By.Id("foo")).Click();
			Assert.AreEqual("clicked", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestIncludingScriptFileRelatively()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script src='test.js'></script>" +
					"</head>" +
					"<body>" +
					"</body>"
				},
				{
					"http://localhost/a/test.js",
					"alert('alert from test.js');"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("alert from test.js", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestIncludingScriptFileRelativelyWithDotDot()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script src='../test.js'></script>" +
					"</head>" +
					"<body>" +
					"</body>"
				},
				{
					"http://localhost/test.js",
					"alert('alert from ../test.js');"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("alert from ../test.js", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestIncludingScriptFileAbsolutely()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script src='/test.js'></script>" +
					"</head>" +
					"<body>" +
					"</body>"
				},
				{
					"http://localhost/test.js",
					"alert('alert from /test.js');"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("alert from /test.js", browser.ScriptHost.Alerts.ToCommaString());
		}

		private SimpleBrowserDriver SetupTest(IDictionary<string, string> pages, string startUrl)
		{
			var factory = new MockWebRequestFactory(pages);

			var browser0 = new Browser(factory);
			var wrapper = new BrowserWrapper(browser0);
			var browser = new SimpleBrowserDriver(wrapper);

			browser.Navigate().GoToUrl(startUrl);
			return browser;
		}
	}
}
