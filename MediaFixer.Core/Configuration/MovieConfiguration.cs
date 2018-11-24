using MediaFixer.Core.IO;
using System;
using System.Collections.Generic;
using MediaFixer.Core.Fixers;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Defines a contract that all movie configurations must implement.
	/// </summary>
	public interface IMovieConfiguration
	{

		/// <summary>
		/// Gets the movie year regex.
		/// </summary>
		String MovieYearRegex { get; }

		/// <summary>
		/// Gets the regular expression used to detect movie names.
		/// </summary>
		String MovieRegex { get; }

		/// <summary>
		/// Gets the characters to replace.
		/// </summary>
		/// <value>
		/// The characters to replace.
		/// </value>
		List<String> CharactersToReplace { get; }

		/// <summary>
		/// Gets the file types supported by the <see cref="IMovieFixer"/>.
		/// </summary>
		List<String> MovieFileTypes { get; }

		/// <summary>
		/// Gets the disqualifying names.
		/// </summary>
		List<String> DisqualifyingNames { get; }
	}

	/// <summary>
	/// Contains settings for the fixing Movies in this application.
	/// </summary>
	/// <seealso cref="MediaFixer.Core.Configuration.BaseConfiguration" />
	public class MovieConfiguration : BaseConfiguration, IMovieConfiguration
	{

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MovieConfiguration"/> class.
		/// </summary>
		/// <param name="appSettingsReader">The application settings reader.</param>
		/// <param name="configurationManager">The configuration manager.</param>
		/// <param name="fileUtility">The file utility.</param>
		public MovieConfiguration(
			IAppSettingsReader appSettingsReader,
			IConfigurationManager configurationManager,
			IFileUtility fileUtility)
			: base(appSettingsReader, configurationManager, fileUtility)
		{
		}


		#endregion CONSTRUCTORS

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the file types supported by the <see cref="IMovieFixer"/>.
		/// </summary>
		public List<String> MovieFileTypes
		{
			get
			{
				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieFileTypes), ".mkv,.iso,.mov,.avi,.m4v,.mp4,.mpg,.wmp,.img");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}

		/// <summary>
		/// Gets the characters to replace.
		/// </summary>
		/// <value>
		/// The characters to replace.
		/// </value>
		public List<String> CharactersToReplace
		{
			get
			{

				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(CharactersToReplace), "[,],{,},(,),~,`,.");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}

		/// <summary>
		/// Gets the disqualifying names.
		/// </summary>
		public List<String> DisqualifyingNames
		{
			get
			{

				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(DisqualifyingNames), "sample");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}

		/// <summary>
		/// Gets the regular expression used to detect movie names.
		/// </summary>
		public String MovieRegex => AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieRegex), @"(?<title>.*)(?<year>19\d{2}|20\d{2})");

		/// <summary>
		/// Gets the movie year regex.
		/// </summary>
		public String MovieYearRegex => AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieYearRegex), @"(19|20)\d{2}");

		/// <summary>
		/// Gets the movie size threshold.
		/// </summary>
		/// <value>
		/// The movie size threshold.
		/// </value>
		public Int64 MovieSizeThreshold => AppSettingsReader.ReadOptionalInt64AppSetting(nameof(MovieSizeThreshold), 1);


		#endregion PUBLIC ACCESSORS

	}
}
