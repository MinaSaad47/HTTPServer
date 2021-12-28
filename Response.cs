using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
	public enum StatusCode
	{
		OK = 200,
		InternalServerError = 500,
		NotFound = 404,
		BadRequest = 400,
		Redirect = 301
	}

	class Response
	{
		string responseString;
		public string ResponseString
		{
			get
			{
				return responseString;
			}
		}
		StatusCode code;
		List<string> headerLines = new List<string>();
		public Response(StatusCode code, string contentType, string content, string redirectoinPath)
		{
			// TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
			responseString = $"{GetStatusLine(code)}\r\n";
			headerLines.Add($"Server: {Configuration.ServerType}");
			headerLines.Add($"Date: {DateTime.Now}");
			// TODO: Create the request string
			headerLines.Add($"Content-Type: {contentType}");
			headerLines.Add($"Content-Length: {content.Length}");
			if (code == StatusCode.Redirect)
			{
				headerLines.Add($"Location: {redirectoinPath}");
			}
			foreach (string header in headerLines)
			{
				responseString += $"{header}\r\n";
			}
			responseString += $"\r\n{content}\r\n\r\n";
		}

		private string GetStatusLine(StatusCode code)
		{
			// TODO: Create the response status line and return it
			Logger.LogConsole($"StatusCode: {(int) code} ({code.ToString()})");
			string statusLine = $"{Configuration.ServerHTTPVersion} {(int) code} {code.ToString()}";
			return statusLine;
		}
	}
}
