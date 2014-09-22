using System.Collections.Generic;
using System.Linq;
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

		[Test]
		public void TestSettingWindowLocation()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script>" +
							"window.location = \"test2.html\";" +
						"</script>" +
					"</head>" +
					"<body>" +
					"</body>"
				},
				{
					"http://localhost/a/test2.html",

					"<head>" +
						"<script>" +
							"alert('alert from test2.html');" +
						"</script>" +
					"</head>" +
					"<body>" +
					"</body>"
				},
			}, "http://localhost/a/test.html");

			Assert.AreEqual("alert from test2.html", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestGettingWindowLocation()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script>" +
							"alert(window.location);" +
						"</script>" +
					"</head>" +
					"<body>" +
					"</body>"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("http://localhost/a/test.html", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestGettingWindowLocationHref()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script>" +
							"alert(window.location.href);" +
						"</script>" +
					"</head>" +
					"<body>" +
					"</body>"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("http://localhost/a/test.html", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestGettingWindowLocationProtocol()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<head>" +
						"<script>" +
							"alert(window.location.protocol);" +
						"</script>" +
					"</head>" +
					"<body>" +
					"</body>"
				}
			}, "http://localhost/a/test.html");

			Assert.AreEqual("http", browser.ScriptHost.Alerts.ToCommaString());
		}

		[Test]
		public void TestSettingWindowLocationUsingAssignMethod()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<script>" +
						"window.location.assign('test2.html');" +
					"</script>"
				},
				{
					"http://localhost/a/test2.html",

					"foobar"
				}

			}, "http://localhost/a/test.html");

			Assert.AreEqual("foobar", browser.PageSource);
		}

		[Test]
		public void TestWindowLocationAddsToNavigationState()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<script>" +
					"function goto_next_page()" +
					"{" +
						"window.location = 'test2.html';" +
					"}" +
					"</script>" +
					"<div id='foo' onclick='goto_next_page()'></div"
				},
				{
					"http://localhost/a/test2.html",

					"foobar"
				}

			}, "http://localhost/a/test.html");

			var depth = browser.Browser.NavigationHistory.Count();

			browser.FindElements(By.TagName("div")).First().Click();
			var depth2 = browser.Browser.NavigationHistory.Count();

			Assert.AreEqual(depth + 1, depth2);
		}

		[Test]
		public void TestGoingBackwardsRemovesFromNavigationState()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<script>" +
					"function goto_next_page()" +
					"{" +
						"window.location = 'test2.html';" +
					"}" +
					"</script>" +
					"<div id='foo' onclick='goto_next_page()'></div>"
				},
				{
					"http://localhost/a/test2.html",

					"foobar"
				}

			}, "http://localhost/a/test.html");

			var depth = browser.Browser.NavigationHistory.Count();

			browser.FindElements(By.TagName("div")).First().Click();
			var depth2 = browser.Browser.NavigationHistory.Count();

			browser.Navigate().Back();

			Assert.AreEqual(depth + 1, depth2);
		}

		[Test]
		public void TestHistoryGoMinusOne()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<div id='foo1' onclick=\"window.location = 'test2.html'\"></div>"
				},
				{
					"http://localhost/a/test2.html",

					"<div id='foo2' onclick='window.history.go(-1)'></div>"
				}

			}, "http://localhost/a/test.html");

			Assert.AreEqual("http://localhost/a/test.html", browser.Url);

			browser.FindElements(By.Id("foo1")).First().Click();
			Assert.AreEqual("http://localhost/a/test2.html", browser.Url);

			browser.FindElements(By.Id("foo2")).First().Click();
			Assert.AreEqual("http://localhost/a/test.html", browser.Url);
		}

		[Test]
		public void TestHistoryGoBack()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<div id='foo1' onclick=\"window.location = 'test2.html'\"></div>"
				},
				{
					"http://localhost/a/test2.html",

					"<div id='foo2' onclick='window.history.back()'></div>"
				}

			}, "http://localhost/a/test.html");

			Assert.AreEqual("http://localhost/a/test.html", browser.Url);

			browser.FindElements(By.Id("foo1")).First().Click();
			Assert.AreEqual("http://localhost/a/test2.html", browser.Url);

			browser.FindElements(By.Id("foo2")).First().Click();
			Assert.AreEqual("http://localhost/a/test.html", browser.Url);
		}

		[Test]
		public void TestHistoryGoForward()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<div id='foo1' onclick=\"window.location = 'test2.html'\"></div>" +
					"<div id='foo3' onclick=\"window.history.forward();\"></div>"
				},
				{
					"http://localhost/a/test2.html",

					"<div id='foo2' onclick='window.history.back()'></div>"
				}

			}, "http://localhost/a/test.html");

			browser.FindElements(By.Id("foo1")).First().Click();
			browser.FindElements(By.Id("foo2")).First().Click();
			browser.FindElements(By.Id("foo3")).First().Click();
			Assert.AreEqual("http://localhost/a/test2.html", browser.Url);
		}

		[Test]
		public void TestHistoryLength()
		{
			var browser = SetupTest(new Dictionary<string, string>
			{
				{
					"http://localhost/a/test.html",

					"<script>alert(window.history.length);</script>" +
					"<div id='foo1' onclick=\"window.location = 'test2.html'\"></div>"
				},
				{
					"http://localhost/a/test2.html",

					"<script>alert(window.history.length);</script>"
				}

			}, "http://localhost/a/test.html");

			browser.FindElements(By.Id("foo1")).First().Click();

			Assert.AreEqual("1,2", browser.ScriptHost.Alerts.ToCommaString());
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
