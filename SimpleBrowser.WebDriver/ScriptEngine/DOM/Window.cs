using System;

namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class Window
	{
		private ScriptHost _host;
		public Window(ScriptHost host)
		{
			_host = host;
		}

		public void alert(string msg)
		{
			_host.LogAlert(msg);
		}
	}
}
