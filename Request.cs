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
				return false;
			// Parse Request line
			requestLines = lines[0].Split(' ');
			// Validate blank line exists
			if (!ValidateBlankLine())
			{
				Logger.LogConsole("ValidateBlankLine Failed");
				return false;
			}
			// Load header lines into HeaderLines dictionary
			LoadHeaderLines();
		}

		private bool ParseRequestLine()
		{
			// throw new NotImplementedException();
			if(requestLines[0] == "GET")
				method = RequestMethod.GET;
			else if (requestLines[0] == "POST")
				method = RequestMethod.POST;
			else if (requestLines[0] == "HEAD")
				method = "HEAD";
			else
			{
				Logger.LogConsole($"Invalid HTTP Method: {requestLines[0]}");
				return false;
			}

			if (!ValidateIsURI(relativeURI[1]))
			{
				Logger.LogConsole($"Invalid Url: {requestLines[0]}");
				return false;
			}

			relativeURI = requestLines[1];

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

		}

		private bool ValidateIsURI(string uri)
		{
			return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
		}

		private bool LoadHeaderLines()
		{
			throw new NotImplementedException();
			for (int i = 1; i < lines.Length - 2; i++)
			{
				string[] dictEntry = lines[i].Split(": ");
				HeaderLines.Add(dictEntry[0], dictEntry[1]);
			}
		}

		private bool ValidateBlankLine()
		{
			// throw new NotImplementedException();
			if (lines[lines.Length - 2] != "\r\n" !!
				lines[lines.Length - 1] != "\r\n")
			{
				return false;
			}
		}

	}
}
