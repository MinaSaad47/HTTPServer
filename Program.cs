using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace HTTPServer
{
	class Program
	{
		static string redirectionRulesPath = "redirectionRules.txt";
		static void Main(string[] args)
		{
			// TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
			CreateRedirectionRulesFile();
            
			//Start server
			// 1) Make server object on port 1000
			Server httpServer = new Server(4747, redirectionRulesPath);
			// 2) Start Server
			httpServer.StartServer();
		}

		static void CreateRedirectionRulesFile()
		{
			// TODO: Create file named redirectionRules.txt
			// each line in the file specify a redirection rule
			// example: "aboutus.html,aboutus2.html"
			// means that when making request to aboustus.html,, it redirects me to aboutus2
			StreamWriter sw = new StreamWriter(redirectionRulesPath);
			sw.WriteLine("aboutus.html,aboutus2.html");
			sw.WriteLine("dir_redir.html,dir/redirection_dir.html");

			sw.Flush();
			sw.Close();
		}
	}
}
