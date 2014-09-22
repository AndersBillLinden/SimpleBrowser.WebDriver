using System;

namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class Window
	{
		private SimpleBrowserDriver _browser;
		private ScriptHost _host;
		private ScriptLog _log;
		public Window(SimpleBrowserDriver browser, ScriptHost host, ScriptLog log)
		{
			_browser = browser;
			_host = host;
			_log = log;
		}

		public void alert(object msg)
		{
			_log.LogAlert(msg.ToString());
		}

		public object location
		{
			get
			{
				return new WindowLocation(_browser, _browser.Browser.Url);
			}
			set
			{
				var uri = new Uri(_browser.Browser.Url, value.ToString());

				_browser.Navigate().GoToUrl(uri);
			}
		}

		public WindowHistory history
		{
			get
			{
				return new WindowHistory(_browser.Browser);
			}
		}
	}
}
