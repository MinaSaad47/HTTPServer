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

		public Server(int portNumber, string redirectionMatrixPath)
		{
			//TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
			this.LoadRedirectionRules();
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
			while (true)
			{
				//TODO: accept connections and start thread for each accepted connection.
				Socket clientSocket = serverSocket.Accept();
				Logger.LogConsole($"Client ({((IPEndPoint)rsock.RemoteEndPoint).
					Address.ToString())}) Connected"):

				Thread clientThread = new Thread(new ParameterizedThreadStart(HandleConnection));
				clientThread.Start(clientSocket);
			}
		}

		public void HandleConnection(object obj)
		{
			// TODO: Create client socket 
			// set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
			Socket clientSocket = (Socket) obj;
			clientSocket.ReceiveTimeout(0);
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
						Logger($"Client ({((IPEndPoint)rsock.RemoteEndPoint).
							Address.ToString())}) Disconnected");
						break;
					}
					// TODO: Create a Request object using received request string
					requestString = Encoding.ASCII.GetString(msg,
															 messageLength);
					Request request = new Request(request);
					// TODO: Call HandleRequest Method that returns the response
					HandleRequest(request);
					// TODO: Send Response back to client

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
			throw new NotImplementedException();
			string content;
			try
			{
				//TODO: check for bad request
				
				//TODO: map the relativeURI in request to get the physical path of the resource.

				//TODO: check for redirect

				//TODO: check file exists

				//TODO: read the physical file

				// Create OK response
			}
			catch (Exception ex)
			{
				// TODO: log exception using Logger class
				Logger.LogException(ex);
				// TODO: in case of exception, return Internal Server Error. 
			}
		}

		private string GetRedirectionPagePathIFExist(string relativePath)
		{
			// using Configuration.RedirectionRules return the redirected page path if exists else returns empty
			
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
				StreamReader sr = new StreamReader(filePath);
				string[] rule = sr.ReadLine().Split(',');
				// then fill Configuration.RedirectionRules dictionary 
				Configuration.RedirectionRules.Add(rule[0], rule[1]);
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
