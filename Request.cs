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
			string[] lines = this.requestString.Split("\r\n");
			// check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
			if (lines.Length < 3)
			{
				Logger.LogConsole($"{lines.Length} lines Only");
				return false;
			}
			foreach (string line in lines)
			{
				Logger.LogConsole($"> {line}");
			}
			// Parse Request line
			requestLines = lines[0].Split(' ');
			if (!ParseRequestLine())
				return false;
			// Validate blank line exists
			// if (!ValidateBlankLine())
			// {
			// 	return false;
			// }
			// Load header lines into HeaderLines dictionary
			if (!LoadHeaderLines())
				return false;

			return true;
		}

		private bool ParseRequestLine()
		{
			// throw new NotImplementedException();
			if(requestLines[0] == "GET")
				method = RequestMethod.GET;
			else if (requestLines[0] == "POST")
				method = RequestMethod.POST;
			else if (requestLines[0] == "HEAD")
				method = RequestMethod.HEAD;
			else
			{
				Logger.LogConsole($"Invalid HTTP Method: {requestLines[0]}");
				return false;
			}

			relativeURI = requestLines[1];
			if (!ValidateIsURI(relativeURI))
			{
				Logger.LogConsole($"Invalid Url: {relativeURI}");
				return false;
			}


			if (requestLines[2] == "HTTP/1.1")
				httpVersion = HTTPVersion.HTTP11;
			else if (requestLines[2] == "HTTP/1.0")
				httpVersion = HTTPVersion.HTTP10;
			else if (requestLines[2] == "HTTP/0.9")
				httpVersion = HTTPVersion.HTTP09;
			else
			{
				Logger.LogConsole($"Invalid HTTP Version: {requestLines[2]}");
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
			for (int i = 1; i < requestLines.Length - 2; i++)
			{
				string[] dictEntry = requestLines[i].Split(": ");
				HeaderLines.Add(dictEntry[0], dictEntry[1]);
			}
			return true;
		}

		private bool ValidateBlankLine()
		{
			throw new NotImplementedException();
			if (!string.Equals(requestLines[requestLines.Length - 2], "\r\n"))
			{
				Logger.LogConsole("ValidateBlankLine Failed");
				return false;
			}

			return true;
		}

	}
}
