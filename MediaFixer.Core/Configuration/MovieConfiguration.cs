using System;
using System.Collections.Generic;
using MediaFixer.Core.Configuration;
using MediaFixer.Core.Fixers;
using MediaFixer.Core.IO;

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
				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieFileTypes), "mkv,iso,mov,avi,m4v,mpg");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}

		/// <summary>
		/// Gets the regular expression used to detect movie names.
		/// </summary>
		public String MovieRegex => AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieRegex), @"(?<title>.*)(?<year>19|20\d{2})");

		/// <summary>
		/// Gets the movie characters to remove.
		/// </summary>
		public List<String> MovieCharactersToRemove
		{
			get
			{
				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieCharactersToRemove), "[,],(,),.,1080p,");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}

		/// <summary>
		/// Gets the movie year regex.
		/// </summary>
		public String MovieYearRegex => AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieYearRegex), @"(19|20)\d{2}");

		/// <summary>
		/// Gets the movie quality markers.
		/// </summary>
		public List<String> MovieQualityMarkers
		{
			get
			{
				var value = AppSettingsReader.ReadOptionalStringAppSetting(nameof(MovieCharactersToRemove), "hevc,bdrip,Bluray,x264,h264,AC3,DTS,480p,720p,1080p");
				var list = value.Split(',');
				return new List<String>(list);
			}
		}


		#endregion PUBLIC ACCESSORS

	}
}
