using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ClearScript.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SimpleBrowser.WebDriver;
using SimpleBrowser;
using System.Collections.ObjectModel;
using Microsoft.ClearScript.V8;
using SimpleBrowser.WebDriver.ScriptEngine;

namespace SimpleBrowser.WebDriver
{
	public class SimpleBrowserDriver : IWebDriver, IHasInputDevices
	{
		IBrowser _my;
		ScriptHost _scriptHost;
		public SimpleBrowserDriver()
		{
			_scriptHost = new ScriptHost(this);
			_my = new BrowserWrapper();
		}
		public SimpleBrowserDriver(IBrowser browser)
		{
			_scriptHost = new ScriptHost(this);
			_my = browser;
		}
		#region IWebDriver Members

		public void Close()
		{
			_my.Close();
			this.Dispose();
		}

		public string CurrentWindowHandle
		{
			get { return _my.WindowHandle; }
		}

		public IOptions Manage()
		{
			return new SimpleManage(this);
		}

		public INavigation Navigate()
		{
			return new SimpleNavigate(this, _my);
		}

		public string PageSource
		{
			get { return _my.CurrentHtml; }
			set { }
		}

		public void Quit()
		{
			this.Close();
		}

		public ITargetLocator SwitchTo()
		{
			return new SimpleTargetLocator(_my);
		}

		public string Title
		{
			get { return _my.Find("title", new object()).Value; }
		}

		public string Url
		{
			get
			{
				return _my.Url.ToString();
			}
			set
			{
				_my.Navigate(value);
			}
		}

		public System.Collections.ObjectModel.ReadOnlyCollection<string> WindowHandles
		{
			get
			{
				return new ReadOnlyCollection<string>(_my.Browsers.Select(b => b.WindowHandle).ToList());
			}
		}

		public Browser Browser
		{
			get
			{
				return _my.GetBrowser();
			}
		}

		public ScriptHost ScriptHost
		{
			get { return _scriptHost; }
		}

		#endregion

		#region ISearchContext Members

		public IWebElement FindElement(By by)
		{
			ISearchContext ctx = CreateSearchContext(_my);
			IWebElement result = by.FindElement(ctx);
			return result;
		}

		private ISearchContext CreateSearchContext(IBrowser my)
		{
			ISearchContext ctx = new PageRoot(my);
			return ctx;
		}

		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			ISearchContext ctx = CreateSearchContext(_my);
			return by.FindElements(ctx);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_my.Close();
		}

		#endregion

		#region IHasInputDevices Members

		private IKeyboard _keyboard;
		public IKeyboard Keyboard
		{
			get {
				if (_keyboard == null)
				{
					_keyboard = new SimpleKeyboard(_my);
				}
				return _keyboard;
			}
		}

		private IMouse _mouse;
		public IMouse Mouse
		{
			get
			{
				if (_mouse == null)
				{
					_mouse = new SimpleMouse(_my);
				}
				return _mouse;
			}
		}

		#endregion

		public void RunScripts()
		{
			var scripts = FindElements(By.TagName("script"));

			foreach(var script in scripts)
			{
				var src = script.GetAttribute("src");

				if (src != null)
				{
					var url = new Uri(_my.Url, src);
					var html = _my.GetBrowser().CreateReferenceView().Fetch(url);

					_scriptHost.AddScriptBlock(html);
				}

				var scriptText = script.Text;

				_scriptHost.AddScriptBlock(scriptText);
			}

			var bodies = FindElements(By.TagName("body"));

			if (bodies.Any())
			{
				var body = bodies.First();

				var onload = body.GetAttribute("onload");

				if (onload != null)
					_scriptHost.AddScriptBlock(onload);
			}

			_my.Clicked += OnClick;
		}

		private void OnClick(Browser browser, HtmlElement element)
		{
			var onclick = element.GetAttributeValue("onclick");

			if (onclick != null)
				_scriptHost.AddScriptBlock(onclick);
		}
	}
}
