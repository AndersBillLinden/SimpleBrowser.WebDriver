using System.Collections.Generic;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using SimpleBrowser.WebDriver.ScriptEngine.DOM;

namespace SimpleBrowser.WebDriver.ScriptEngine
{
	public class ScriptHost
	{
		public V8ScriptEngine _engine;
		public SimpleBrowserDriver _browser;

		public IEnumerable<string> Alerts { get { return _alerts; } }
		private List<string> _alerts = new List<string>();

		public IEnumerable<string> ConsoleLogs { get { return _consoleLogs; } }
		private List<string> _consoleLogs = new List<string>();

	
		public ScriptHost(SimpleBrowserDriver browser)
		{
			_browser = browser;

			var window = new Window(_browser, this);

			_engine = new V8ScriptEngine();
			_engine.AddHostObject("window", HostItemFlags.GlobalMembers, window);
			_engine.AddHostObject("document", new Document(_browser));
			_engine.AddHostObject("console", new Console(this));
		}

		public void AddScriptBlock(string html)
		{
			_engine.Execute(html);
		}

		public void LogAlert(string msg)
		{
			_alerts.Add(msg);
		}

		public void LogConsoleLog(string msg)
		{
			_consoleLogs.Add(msg);
		}
	}
}
