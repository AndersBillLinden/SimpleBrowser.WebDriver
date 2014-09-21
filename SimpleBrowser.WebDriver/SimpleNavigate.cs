using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
	public class SimpleNavigate : INavigation
	{
		private readonly SimpleBrowserDriver _driver;
		private readonly IBrowser _browser;

		private readonly List<string> _urlCache = new List<string>();

		public SimpleNavigate(SimpleBrowserDriver driver, IBrowser browser)
		{
			_driver = driver;
			_browser = browser;
		}


		#region INavigation Members

		public void Back()
		{
			_browser.NavigateBack();
			_driver.RunScripts();
		}

		public void Forward()
		{
			_browser.NavigateForward();
			_driver.RunScripts();
		}

		public void GoToUrl(Uri url)
		{
			if (url == null) return;
			GoToUrl(url.ToString());
		}

		public void GoToUrl(string url)
		{
			if (url == null) return;
			_browser.Navigate(url);
			_driver.RunScripts();
		}

		public void FetchUrl(Uri url)
		{
			if (url == null) return;
			FetchUrl(url.ToString());
		}

		public void FetchUrl(string url)
		{
			if (url == null) return;
			_browser.Navigate(url);
		}

		public void Refresh()
		{
			var url = _browser.Url;
			_browser.NavigateBack();
			_browser.Navigate(url.ToString());
			_driver.RunScripts();
		}

		#endregion

	}
}
