using System.Collections.Generic;
using System.IO;
using System.Net;
using Moq;
using SimpleBrowser.Network;
using System;

namespace DriverTest.Helpers
{
	public class MockWebRequestFactory: IWebRequestFactory
	{
		IDictionary<string, string> _pages;

		public MockWebRequestFactory(IDictionary<string, string> pages)
		{
			_pages = pages;
		}

		public IHttpWebRequest GetWebRequest(Uri url)
		{
			var page = _pages[url.AbsoluteUri];

			var request = new Mock<IHttpWebRequest>();
			request.SetupProperty(m => m.Headers, new WebHeaderCollection());
			request.SetupProperty(m=>m.Host, url.Host);

			var response = new Mock<IHttpWebResponse>();

			var requestStream = new MemoryStream();
				
			request.Setup(m => m.GetRequestStream()).Returns(requestStream);

			if (page != null)
			{
				response.SetupProperty(m => m .StatusCode, HttpStatusCode.OK);
			}
			else
			{
				response.SetupProperty(m => m .StatusCode, HttpStatusCode.NotFound);
				page = "";
			}

			response.Setup(m => m.GetResponseStream()).Returns(new StringStream(page));

			request.SetupProperty(m=>m.ContentLength, page.Length);
			request.Setup(m => m.GetResponse()).Returns(response.Object);

			return request.Object;
		}
	}
}
