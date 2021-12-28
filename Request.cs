using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
	public enum RequestMethod
	{
		GET,
		POST,
		HEAD
	}

	public enum HTTPVersion
	{
		HTTP10,
		HTTP11,
		HTTP09
	}

	class Request
	{
		string[] requestLines;
		RequestMethod method;
		public string relativeURI;
		Dictionary<string, string> headerLines;

		public Dictionary<string, string> HeaderLines
		{
			get { return headerLines; }
		}

		HTTPVersion httpVersion;
		string requestString;
		string[] contentLines;

		public Request(string requestString)
		{
			this.requestString = requestString;
		}
		/// <summary>
		/// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
		/// </summary>
		/// <returns>True if parsing succeeds, false otherwise.</returns>
		public bool ParseRequest()
		{
			// throw new NotImplementedException();

			//TODO: parse the receivedRequest using the \r\n delimeter
			requestLines = this.requestString.Split("\r\n");
			// check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
			if (requestLines.Length < 3)
			{
				Logger.LogConsole($"{requestLines.Length} lines Only");
				return false;
			}
			// Parse Request line
			if (!ParseRequestLine())
				return false;
			// Validate blank line exists
			if (!ValidateBlankLine())
			{
				return false;
			}
			// Load header lines into HeaderLines dictionary
			if (!LoadHeaderLines())
				return false;

			return true;
		}

		private bool ParseRequestLine()
		{
			// throw new NotImplementedException();
			string[] reqLine = requestLines[0].Split(' ');
			if(reqLine[0] == "GET")
				method = RequestMethod.GET;
			else if (reqLine[0] == "POST")
				method = RequestMethod.POST;
			else if (reqLine[0] == "HEAD")
				method = RequestMethod.HEAD;
			else
			{
				Logger.LogConsole($"Invalid HTTP Method: {reqLine[0]}");
				return false;
			}

			relativeURI = reqLine[1];
			if (!ValidateIsURI(relativeURI))
			{
				Logger.LogConsole($"Invalid Url: {relativeURI}");
				return false;
			}


			if (reqLine[2] == "HTTP/1.1")
				httpVersion = HTTPVersion.HTTP11;
			else if (reqLine[2] == "HTTP/1.0")
				httpVersion = HTTPVersion.HTTP10;
			else if (reqLine[2] == "HTTP/0.9")
				httpVersion = HTTPVersion.HTTP09;
			else
			{
				Logger.LogConsole($"Invalid HTTP Version: {reqLine[2]}");
				return false;
			}
			return true;
		}

		private bool ValidateIsURI(string uri)
		{
			return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
		}

		private bool LoadHeaderLines()
		{
			// throw new NotImplementedException();
			headerLines = new Dictionary<string, string>();
			for (int i = 1; i < requestLines.Length - 2; i++)
			{
				// Logger.LogConsole($"(Header) {requestLines[i]}");
				string[] dictEntry = requestLines[i].Split(": ");
				HeaderLines.Add(dictEntry[0], dictEntry[1]);
			}
			return true;
		}

		private bool ValidateBlankLine()
		{
			// throw new NotImplementedException();
			if (requestLines[requestLines.Length - 2] != String.Empty)
			{
				Logger.LogConsole("ValidateBlankLine Failed");
				return false;
			}

			return true;
		}

	}
}
