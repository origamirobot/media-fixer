using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MediaFixer
{

	/// <summary>
	/// Main class for the MediaFixer application.
	/// </summary>
	public class Program
	{

		/// <summary>
		/// Entry-point into this application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		static void Main(String[] args)
		{
			var pattern = "(.*)([19,20]\\d{3,})";
			var regex = new Regex(pattern);

			//var currentFolder = @"C:\Users\Chris\Desktop\TestFolder";
			var currentFolder = Environment.CurrentDirectory;
			var folders = Directory.GetDirectories(currentFolder);
			foreach (var folder in folders)
			{
				var dir = new DirectoryInfo(folder);
				var match = regex.Match(dir.Name);
				if (match.Groups.Count < 2)
					continue;

				var movieName = RemoveChars(match.Groups[1].Value);
				var year = match.Groups[2].Value;

				var newName = $"{movieName} ({year})";
				var newFullPath = Path.Combine(dir.Parent.FullName, newName);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Renaming ");
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(dir.Name);
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" to ");
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine(newName);

				if(dir.FullName != newFullPath)
					Directory.Move(dir.FullName, newFullPath);
			}
			Console.ReadLine();
		}

		static String RemoveChars(String text)
		{
			text = text.Replace(".", " ")
				.Replace("_", " ")
				.Replace("(", "")
				.Replace(")", "")
				.Trim();
			return text;
		}
			
	}
}
