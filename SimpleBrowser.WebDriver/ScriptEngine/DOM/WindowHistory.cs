namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class WindowHistory
	{
		Browser _browser;
		public WindowHistory(Browser browser)
		{
			_browser = browser;
		}

		public void go(int num)
		{
			if (num < 0)
			{
				for(var i = 0; i > num; i--)
				{
					_browser.NavigateBack();
				}
			}
			else if (num > 0)
			{
				for(var i = 0; i < num; i++)
				{
					_browser.NavigateForward();
				}
			}
		}

		public void back()
		{
			_browser.NavigateBack();
		}

		public void forward()
		{
			_browser.NavigateForward();
		}

		public int length
		{
			get
			{
				return _browser.NavigationHistory.Count;
			}
		}
	}
}
