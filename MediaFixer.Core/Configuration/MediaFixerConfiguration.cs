using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
using System;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Contains settings for the MediaFixer application.
	/// </summary>
	/// <seealso cref="MediaFixer.Core.Configuration.BaseConfiguration" />
	public class MediaFixerConfiguration : BaseConfiguration, IMediaFixerConfiguration
	{

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFixerConfiguration"/> class.
		/// </summary>
		/// <param name="appSettingsReader">The application settings reader.</param>
		/// <param name="configurationManager">The configuration manager.</param>
		/// <param name="fileUtility">The file utility.</param>
		public MediaFixerConfiguration(
			IAppSettingsReader appSettingsReader,
			IConfigurationManager configurationManager,
			IFileUtility fileUtility)
			: base(appSettingsReader, configurationManager, fileUtility)
		{
		}


		#endregion CONSTRUCTORS

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the log level.
		/// </summary>
		public LogLevel LogLevel => AppSettingsReader.ReadOptionalEnumAppSetting<LogLevel>(nameof(LogLevel), LogLevel.Debug);

		/// <summary>
		/// Gets or sets the name of the logger.
		/// </summary>
		public String LoggerName => AppSettingsReader.ReadOptionalStringAppSetting(nameof(LoggerName), "MediaFixer");



		#endregion PUBLIC ACCESSORS

	}
}
