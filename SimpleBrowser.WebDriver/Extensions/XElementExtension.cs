using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using SimpleBrowser.Parser;

namespace SimpleBrowser.WebDriver
{
	public static class XElementExtension
	{
		private static readonly HashSet<string> VoidElements = new HashSet<string>
		{
			"area", "base", "br", "col", "command", "embed", "hr", "img", "input",
			"keygen", "link", "meta", "param", "source", "track", "wbr"
		};

		public static string GetInnerHtml(this XElement element)
		{
			var result = "";

			var subNodes = element.Nodes();

			foreach (var n in subNodes)
			{
				if (n.NodeType == XmlNodeType.Text)
					result += n.ToString();
				else if (n.NodeType == XmlNodeType.Element)
				{
					var e = n as XElement;

					if (e != null)
					{
						result += e.OuterHtml();
					}
				}
			}

			return result;
		}

		public static string OuterHtml(this XElement element)
		{
			var result = "<" + element.Name;

			var attrs = element.Attributes();
			if (attrs.Any())
			{
				result += " " + string.Join(" ",
					attrs.Select(a=> a.Name + "=\"" + HttpUtility.HtmlEncode(a.Value) + "\""));
			}

			result += ">";

			var subNodes = element.Nodes();

			if (subNodes.Any())
			{
				result += element.GetInnerHtml() + "</" + element.Name + ">";
			}
			else
			{
				if (!VoidElements.Contains(element.Name.ToString().ToLower()))
				{
					result += "</" + element.Name + ">";
				}
			}

			return result;
		}

		public static void SetInnerHtml(this XElement element, string html)
		{
			var innerElement = html.ParseHtml();

			element.RemoveAll();

			foreach(var node in innerElement.Root.Nodes())
			{
				element.Add(node);
			}
		}
	}
}
