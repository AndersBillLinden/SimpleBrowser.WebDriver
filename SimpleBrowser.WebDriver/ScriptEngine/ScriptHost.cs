using System.Collections.Generic;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using SimpleBrowser.WebDriver.ScriptEngine.DOM;

namespace SimpleBrowser.WebDriver.ScriptEngine
{
	public class ScriptHost
	{
		public V8ScriptEngine _engine;
	
		public ScriptHost(SimpleBrowserDriver browser)
		{
			var window = new Window(browser, browser.ScriptHost, browser.ScriptLog);

			_engine = new V8ScriptEngine();
			_engine.AddHostObject("window", HostItemFlags.GlobalMembers, window);
			_engine.AddHostObject("document", new Document(browser));
			_engine.AddHostObject("console", new Console(browser.ScriptLog));
		}

		public void AddScriptBlock(string html)
		{
			_engine.Execute(html);
		}
	}
}
