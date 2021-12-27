using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace HTTPServer
{
	class Logger
	{
		static StreamWriter sw = new StreamWriter("log.txt");
		public static void LogException(Exception ex)
		{
			// TODO: Create log file named log.txt to log exception details in it
			//Datetime:
			//message:
			// for each exception write its details associated with datetime 
			sw.WriteLine($"[{DateTime.Now}] {ex.ToString()}");
			LogConsole(ex.ToString());
			sw.Flush();
		}
		public static void LogConsole(string str)
		{
			Console.WriteLine($"[{DateTime.Now}] {str}");
		}
	}
}
