using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOC
{
	public static class HelperMethods
	{
		public static int Year { get; set; }
		public static Assembly ExectuingAssembly { get; set; }
		
		/// <summary>Gets the input data for a specified day</summary>
		public static string GetInput(int day)
		{
			string resourceName = $"_{Year}.Input.{day}.txt";

			string[] embeddedResources = ExectuingAssembly.GetManifestResourceNames();
			if (!embeddedResources.Contains(resourceName))
			{
				Console.WriteLine(@$"Could Not Find ""{resourceName}""");
				return null;
			}

			using (var reader = new StreamReader(stream: ExectuingAssembly.GetManifestResourceStream(resourceName)))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
