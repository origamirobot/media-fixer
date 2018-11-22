﻿using MediaFixer.Core.Configuration;
using System;
using System.IO;
using System.Text.RegularExpressions;
using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
using MediaFixer.Core.Models;

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


		#endregion PROTECTED PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MovieFixer" /> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="directoryUtility">The directory utility.</param>
		/// <param name="fileUtility">The file utility.</param>
		public MovieFixer(
			IMovieConfiguration settings, 
			ILogger logger, 
			IDirectoryUtility directoryUtility, 
			IFileUtility fileUtility)
		{
			Settings = settings;
			Logger = logger;
			DirectoryUtility = directoryUtility;
			FileUtility = fileUtility;
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


		#endregion PROTECTED METHODS

		#region PUBLIC METHODS



		/// <summary>
		/// Parses the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public MovieResult Parse(String location)
		{
			try
			{
				if (!DirectoryUtility.Exists(location))
					throw new FileNotFoundException($"Couldn't find directory {location}");

				var filename = DirectoryUtility.GetDirectoryName(location);

				var match = MovieRegex.Match(filename);
				if (!match.Success)
					throw new Exception($"Regular expression couldn't match ${filename} using the regular expression {Settings.MovieRegex}");

				var titleGroup = match.Groups["title"];
				var yearGroup = match.Groups["year"];
				

				if (String.IsNullOrEmpty(titleGroup?.Value))
					throw new Exception($"Regular expression couldn't match the title for ${filename} using the regular expression {Settings.MovieRegex}");

				if (String.IsNullOrEmpty(yearGroup?.Value))
					throw new Exception($"Regular expression couldn't match the year for ${filename} using the regular expression {Settings.MovieRegex}");

				if (!Int32.TryParse(yearGroup.Value, out var year))
					throw new Exception($"Couldn't parse the year \"${yearGroup.Value}\" because it is not a valid integer");

				return new MovieResult()
				{
					Title = titleGroup.Value.Replace("."," ").Trim(),
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
		public void FixDirectory(String location)
		{
			try
			{
				if (!DirectoryUtility.Exists(location))
					throw new FileNotFoundException($"Couldn't find directory {location}");

				var di = DirectoryUtility.GetDirectoryInfo(location);
				var filename = di.Name;

				var match = MovieRegex.Match(filename);
				if (!match.Success)
					throw new Exception($"Regular expression couldn't match ${filename} using the regular expression {Settings.MovieRegex}");

				var title = match.Groups["title"];
				var year = match.Groups["year"];

				if(String.IsNullOrEmpty(title?.Value))
					throw new Exception($"Regular expression couldn't match the title for ${filename} using the regular expression {Settings.MovieRegex}");

				if (String.IsNullOrEmpty(year?.Value))
					throw new Exception($"Regular expression couldn't match the year for ${filename} using the regular expression {Settings.MovieRegex}");


			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		/// <summary>
		/// Fixes the file.
		/// </summary>
		/// <param name="location">The location.</param>
		public void FixFile(String location)
		{
			try
			{

			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}


		#endregion PUBLIC METHODS


	}


}