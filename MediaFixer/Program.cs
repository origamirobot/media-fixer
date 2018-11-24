using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediaFixer.Core.Fixers;
using MediaFixer.Core.IO;
using MediaFixer.Core.Terminal;
using Ninject;

namespace MediaFixer
{

	/// <summary>
	/// Main class for the MediaFixer application.
	/// </summary>
	public class Program
	{

		/// <summary>
		/// Gets or sets the dependency injection kernel.
		/// </summary>
		private static IKernel Kernel { get; set; }

		/// <summary>
		/// Gets or sets the directory utility.
		/// </summary>
		private static IDirectoryUtility DirectoryUtility { get; set; }

		/// <summary>
		/// Gets or sets the movie fixer.
		/// </summary>
		private static IMovieFixer MovieFixer { get; set; }


		/// <summary>
		/// Entry-point into this application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		private static void Main(String[] args)
		{
			Kernel = new StandardKernel();
			Bootstrapper.Register(Kernel);
			DirectoryUtility = Kernel.Get<IDirectoryUtility>();
			MovieFixer = Kernel.Get<IMovieFixer>();

			Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);



			start:
			Console.Clear();
			var banner1 = new ConsoleBanner("MEDIA FIXER", "Arial", 8, FontStyle.Bold, 150, 14) { ForeColor = ConsoleColor.Blue, Pallet = new Char[] { '#', '%', 'M', 'V', 'l', ',', '.', ' ' } };
			banner1.Execute();
			
			var folders = DirectoryUtility.GetDirectories(Environment.CurrentDirectory);
			foreach (var folder in folders)
			{
				MovieFixer.Fix(folder);
			}

			Console.ReadLine();

		}

	}
}
