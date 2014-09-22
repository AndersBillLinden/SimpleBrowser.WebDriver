namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class Console
	{
		private ScriptLog _log;
		public Console(ScriptLog log)
		{
			_log = log;
		}

		public void log(string msg)
		{
			_log.LogConsoleLog(msg);
		}
	}
}
