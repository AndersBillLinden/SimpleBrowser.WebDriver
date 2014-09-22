using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBrowser.WebDriver.ScriptEngine
{
	public class ScriptLog
	{
		public IEnumerable<string> Alerts { get { return _alerts; } }
		private List<string> _alerts = new List<string>();

		public IEnumerable<string> ConsoleLogs { get { return _consoleLogs; } }
		private List<string> _consoleLogs = new List<string>();

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
