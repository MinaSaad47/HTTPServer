using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Threading;

namespace HTTPServer
{
	class Server
	{
		Socket serverSocket;
		const int messageCapacity = 2048;
		int portNumber;

		public Server(int portNumber, string redirectionMatrixPath)
		{
			this.portNumber = portNumber;
			//TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
			LoadRedirectionRules(redirectionMatrixPath);
			//TODO: initialize this.serverSocket
			try
			{
				serverSocket = new Socket(AddressFamily.InterNetwork,
										  SocketType.Stream,
										  ProtocolType.Tcp);
				serverSocket.Bind(new IPEndPoint(IPAddress.Any, portNumber));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}

		public void StartServer()
		{
			// TODO: Listen to connections, with large backlog.
			serverSocket.Listen(128);
			// TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
			Logger.LogConsole($"Listenning on {((IPEndPoint) serverSocket.RemoteEndPoint).Address.ToString()}:{portNumber}");
			while (true)
			{
				//TODO: accept connections and start thread for each accepted connection.
				Socket clientSocket = serverSocket.Accept();
				Logger.LogConsole($"Client ({((IPEndPoint) clientSocket.RemoteEndPoint).Address.ToString()}) Connected");

				Thread clientThread = new Thread(new ParameterizedThreadStart(HandleConnection));
				clientThread.Start(clientSocket);
			}
		}

		public void HandleConnection(object obj)
		{
			// TODO: Create client socket 
			// set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
			Socket clientSocket = (Socket) obj;
			clientSocket.ReceiveTimeout = 0;
			byte[] msg = new byte[messageCapacity];
			string requestString;
			
			// TODO: receive requests in while true until remote client closes the socket.
			while (true)
			{
				try
				{
					// TODO: Receive request
					int messageLength = clientSocket.Receive(msg);
					// TODO: break the while loop if receivedLen==0
					if (messageLength == 0)
					{
						Logger.LogConsole($"Client ({((IPEndPoint) clientSocket.RemoteEndPoint).Address.ToString()}) Disconnected");
						break;
					}
					// TODO: Create a Request object using received request string
					requestString = Encoding.ASCII.GetString(msg, 0,
															 messageLength);
					Request request = new Request(requestString);
					// TODO: Call HandleRequest Method that returns the response
					Response response = HandleRequest(request);
					// TODO: Send Response back to client
					clientSocket.Send(Encoding.ASCII.GetBytes(response.ResponseString));
				}
				catch (Exception ex)
				{
					// TODO: log exception using Logger class
					Logger.LogException(ex);
				}
			}

			// TODO: close client socket
			clientSocket.Close();
		}

		Response HandleRequest(Request request)
		{
			// throw new NotImplementedException();
			StatusCode code;
			string contentType = "text/html";
			string content;
			string redirectionPath;
			Response response;
			bool isBadRequest;
			string filePath = Configuration.RootPath;
			try
			{
				//TODO: check for bad request
				if (request.ParseRequest())
				{
					code = StatusCode.OK;
				//TODO: map the relativeURI in request to get the physical path of the resource.
					filePath += request.relativeURI;
				//TODO: check for redirect
					filePath = GetRedirectionPagePathIFExist(request.relativeURI);
				//TODO: check file exists
					if (File.Exists(filePath) &&
						!String.IsNullOrEmpty(filePath))
					{
				//TODO: read the physical file
						filePath += request.relativeURI;
						Logger.LogConsole($"File Path: {request.relativeURI}");
					}
					content = File.ReadAllText(filePath);
				//TODO Create OK response
					response = new Response(code, contentType, content,
											filePath);
					Logger.LogConsole("Good Request");
					return response;
				}
				else
				{
					code = StatusCode.BadRequest;
					filePath += $"/{Configuration.BadRequestDefaultPageName}";
					content = File.ReadAllText(filePath);
					response = new Response(code, contentType, content,
											string.Empty);
					Logger.LogConsole("Bad Request");
					return response;
				}
			}
			catch (Exception ex)
			{
				// TODO: log exception using Logger class
				Logger.LogException(ex);
				// TODO: in case of exception, return Internal Server Error. 
				filePath += $"/{Configuration.InternalErrorDefaultPageName}";
				content = File.ReadAllText(filePath);
				response = new Response(StatusCode.InternalServerError,
										contentType, content,
										string.Empty);
				return response;
			}
		}

		private string GetRedirectionPagePathIFExist(string relativePath)
		{
			// using Configuration.RedirectionRules return the redirected page path if exists else returns empty
			if (Configuration.RedirectionRules.ContainsKey(relativePath))
			{
				Logger.LogConsole($"Redirection: {relativePath}");
				return $"{Configuration.RootPath}/{relativePath}";
			}
			return string.Empty;
		}

		private string LoadDefaultPage(string defaultPageName)
		{
			string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
			// TODO: check if filepath not exist log exception using Logger class and return empty string
			
			// else read file and return its content
			return string.Empty;
		}

		private void LoadRedirectionRules(string filePath)
		{
			try
			{
				// TODO: using the filepath paramter read the redirection rules from file 
				// then fill Configuration.RedirectionRules dictionary 
				Configuration.RedirectionRules = new Dictionary<string, string>();
				foreach (string line in File.ReadLines(filePath))
				{
					string[] rule = line.Split(',');
					Logger.LogConsole($"Rule: {rule[0]} => {rule[1]}");
					Configuration.RedirectionRules.Add(rule[0], rule[1]);
				}
			}
			catch (Exception ex)
			{
				// TODO: log exception using Logger class
				Logger.LogException(ex);
				Environment.Exit(1);
			}
		}
	}
}
