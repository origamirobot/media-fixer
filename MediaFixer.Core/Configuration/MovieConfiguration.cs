using MediaFixer.Core.Configuration;
using MediaFixer.Core.IO;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Defines a contract that all movie configurations must implement.
	/// </summary>
	public interface IMovieConfiguration
	{

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






		#endregion PUBLIC ACCESSORS

	}
}
