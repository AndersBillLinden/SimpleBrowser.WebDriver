using System.Collections.Generic;

namespace DriverTest.Extensions
{
	public static class IEnumerableStringsExtension
	{
		public static string ToCommaString(this IEnumerable<string> input)
		{
			return string.Join(",", input);
		}
	}
}
