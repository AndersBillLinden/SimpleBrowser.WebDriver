using System;

namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class WindowLocation
	{
		Uri _uri;
		SimpleBrowserDriver _browser;

		public WindowLocation(SimpleBrowserDriver browser, Uri uri)
		{
			_browser = browser;
			_uri = uri;
		}

		public string href
		{
			get
			{
				return _uri.ToString();
			}
		}

		public string hostname
		{
			get
			{
				return _uri.Host;
			}
		}

		public string pathname
		{
			get
			{
				return _uri.AbsolutePath;
			}
		}

		public string protocol
		{
			get
			{
				return _uri.Scheme;
			}
		}

		public void assign(string uri)
		{
			var destinationUrl = new Uri(_uri, uri);

			_browser.Navigate().GoToUrl(destinationUrl);
		}

		public override string ToString()
		{
			return _uri.ToString();
		}
	}
}
