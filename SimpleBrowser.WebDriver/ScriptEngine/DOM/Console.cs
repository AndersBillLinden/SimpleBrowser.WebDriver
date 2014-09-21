namespace SimpleBrowser.WebDriver.ScriptEngine.DOM
{
	public class Console
	{
		private ScriptHost _host;
		public Console(ScriptHost host)
		{
			_host = host;
		}

		public void log(string msg)
		{
			_host.LogConsoleLog(msg);
		}
	}
}
