using MediaFixer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MediaFixer.Core.Extensions;
using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
using MediaFixer.Core.Models;
using MediaFixer.Core.Terminal;

namespace MediaFixer.Core.Fixers
{

	/// <summary>
	/// Defines a contract that all movie fixers must implement.
	/// </summary>
	/// <seealso cref="IFixer" />
	public interface IMovieFixer : IFixer
	{

		/// <summary>
		/// Parses the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		MovieResult Parse(String location);

	}


	/// <summary>
	/// Responsible for fixing movies.
	/// </summary>
	public class MovieFixer : IMovieFixer
	{

		#region PRIVATE PROPERTIES


		private String _entryDirectory;


		#endregion PRIVATE PROPERTIES

		#region PROTECTED PROPERTIES


		/// <summary>
		/// Gets the settings.
		/// </summary>
		protected IMovieConfiguration Settings { get; private set; }

		/// <summary>
		/// Gets the logger.
		/// </summary>
		protected ILogger Logger { get; private set; }

		/// <summary>
		/// Gets the year regex.
		/// </summary>
		protected Regex YearRegex { get; private set; }

		/// <summary>
		/// Gets the movie regex.
		/// </summary>
		protected Regex MovieRegex { get; private set; }

		/// <summary>
		/// Gets the directory utility.
		/// </summary>
		protected IDirectoryUtility DirectoryUtility { get; private set; }

		/// <summary>
		/// Gets the file utility.
		/// </summary>
		protected IFileUtility FileUtility { get; private set; }

		/// <summary>
		/// Gets the path utility.
		/// </summary>
		protected IPathUtility PathUtility { get; private set; }

		/// <summary>
		/// Gets the console.
		/// </summary>
		protected IConsole Console { get; private set; }


		#endregion PROTECTED PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MovieFixer" /> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="directoryUtility">The directory utility.</param>
		/// <param name="fileUtility">The file utility.</param>
		/// <param name="pathUtility">The path utility.</param>
		/// <param name="console">The console.</param>
		public MovieFixer(
			IMovieConfiguration settings, 
			ILogger logger, 
			IDirectoryUtility directoryUtility, 
			IFileUtility fileUtility, 
			IPathUtility pathUtility, 
			IConsole console)
		{
			Settings = settings;
			Logger = logger;
			DirectoryUtility = directoryUtility;
			FileUtility = fileUtility;
			PathUtility = pathUtility;
			Console = console;
			YearRegex = new Regex(Settings.MovieYearRegex);
			MovieRegex = new Regex(Settings.MovieRegex);
		}


		#endregion CONSTRUCTORS

		#region PROTECTED METHODS


		/// <summary>
		/// Finds the year from the specified input.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		protected Int32? FindYear(String input)
		{
			var match = YearRegex.Match(input);
			if (!match.Success)
				return null;

			if (Int32.TryParse(match.Value, out var year))
				return year;

			return null;
		}

		/// <summary>
		/// Gets a list of every file under the specified directory that can be considered a movie file.
		/// </summary>
		/// <param name="location">The location to search for movie files.</param>
		/// <returns></returns>
		protected List<MovieResult> GetMovieFiles(String location)
		{
			try
			{
				var movies = new List<MovieResult>();
				var parent = DirectoryUtility.GetDirectoryInfo(_entryDirectory);
				var files = DirectoryUtility.GetFiles(location);
				foreach (var file in files)
				{
					var fi = FileUtility.GetFileInfo(file);
					if (!Settings.MovieFileTypes.Contains(fi.Extension))
						continue;

					var disqualified = false;
					foreach (var word in Settings.DisqualifyingNames)
					{
						if (!fi.Name.Contains(word))
							continue;

						disqualified = true;
						break;
					}

					if (disqualified)
						continue;


					var result = Parse(parent.Name);
					result.Length = fi.Length;
					result.Path = file;
					result.Extension = fi.Extension;
					movies.Add(result);
				}

				var folders = DirectoryUtility.GetDirectories(location);
				foreach (var folder in folders)
				{
					var subMovies = GetMovieFiles(folder);
					movies.AddRange(subMovies);
				}
				return movies;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
		}

		/// <summary>
		/// Looks for the movie file in the specified location.
		/// </summary>
		/// <param name="location">The input.</param>
		/// <returns></returns>
		protected MovieResult FindMovieFile(String location)
		{
			try
			{
				_entryDirectory = location;
				var movies = GetMovieFiles(location);
				
				if (movies == null || movies.Count == 0)
					throw new MovieNotFoundException($"Coldn't find a movie file in {location}");


				// TODO: Parse for two part movies here
				

				// FIND THE LARGEST MOVIE FILE, IT SHOULD BE THE CORRECT ONE
				var largest = movies.OrderByDescending(x => x.Length).First();
				return largest;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
		}


		#endregion PROTECTED METHODS

		#region PUBLIC METHODS


		/// <summary>
		/// Parses the specified location.
		/// </summary>
		/// <param name="name">The location.</param>
		/// <returns></returns>
		public MovieResult Parse(String name)
		{
			try
			{
				var match = MovieRegex.Match(name);
				if (!match.Success)
					throw new Exception($"Regular expression couldn't match ${name} using the regular expression {Settings.MovieRegex}");

				var titleGroup = match.Groups["title"];
				var yearGroup = match.Groups["year"];
				
				if (String.IsNullOrEmpty(titleGroup?.Value))
					throw new Exception($"Regular expression couldn't match the title for ${name} using the regular expression {Settings.MovieRegex}");

				if (String.IsNullOrEmpty(yearGroup?.Value))
					throw new Exception($"Regular expression couldn't match the year for ${name} using the regular expression {Settings.MovieRegex}");

				if (!Int32.TryParse(yearGroup.Value, out var year))
					throw new Exception($"Couldn't parse the year \"${yearGroup.Value}\" because it is not a valid integer");


				var title = titleGroup.Value;
				foreach (var c in Settings.CharactersToReplace)
					title = title.Replace(c, " ");

				title = title.Replace("  ", " ").ToTitleCase();

				return new MovieResult()
				{
					Title = title.Trim(),
					Year = year
				};
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
		}

		/// <summary>
		/// Fixes the directory.
		/// </summary>
		/// <param name="location">The location.</param>
		public void Fix(String location)
		{
			try
			{
				var textColor = ConsoleColor.DarkGray;
				var varColor = ConsoleColor.Blue;

				Console.Write("Parsing ", ConsoleColor.White);
				Console.WriteLine(location, varColor);

				if (!DirectoryUtility.Exists(location))
					throw new FileNotFoundException($"Couldn't find directory {location}");

				var di = DirectoryUtility.GetDirectoryInfo(location);

				if (di.Parent == null)
					throw new FileNotFoundException($"Couldn't find a parent directory for {location}");


				var movie = FindMovieFile(location);
				var fixedName = $"{movie.Title} ({movie.Year})";
				var fixedFile = $"{fixedName}{movie.Extension}";

				if (di.Name == fixedName)
				{
					Console.WriteLine("");
					Console.WriteLine($"      ALREADY FIXED!", ConsoleColor.Green);
					Console.WriteLine("");
					return;
				}

				Console.Write($"    - 1/5) Found movie file at ", textColor);
				Console.WriteLine(movie.Path, varColor);


				Console.Write("    - 2/5) New movie title is ", textColor);
				Console.WriteLine(fixedName, varColor);


				var path = PathUtility.Combine(di.Parent.FullName, fixedName);
				DirectoryUtility.CreateDirectory(path);

				Console.Write("    - 3/5) Created new directory ", textColor);
				Console.WriteLine(path, varColor);

				var destination = Path.Combine(di.Parent.FullName, fixedName, fixedFile);
				FileUtility.Move(movie.Path, destination);

				Console.Write($"    - 4/5) Movied movie file to ", textColor);
				Console.WriteLine(path, varColor);

				DirectoryUtility.Delete(di.FullName, true);

				Console.Write("    - 5/5) Removed directory ", textColor);
				Console.WriteLine(di.FullName, varColor);

				Console.WriteLine("");
				Console.WriteLine($"      DONE!", ConsoleColor.Green);
				Console.WriteLine("");
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				Console.Write($"    - ERROR - ", ConsoleColor.Red);
				Console.WriteLine(ex.Message, ConsoleColor.DarkRed);
			}
		}


		#endregion PUBLIC METHODS

	}

}
