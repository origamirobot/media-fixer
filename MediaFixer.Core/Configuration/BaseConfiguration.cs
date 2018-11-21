using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaFixer.Core.IO;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseConfiguration 
	{

		#region PRIVATE PROPERTIES


		/// <summary>
		/// Gets the application settings reader thats used to read settings from the configuration files.
		/// </summary>
		protected virtual IAppSettingsReader AppSettingsReader { get; private set; }

		/// <summary>
		/// Gets the configuration manager.
		/// </summary>
		protected virtual IConfigurationManager ConfigurationManager { get; private set; }

		/// <summary>
		/// Gets the file utility.
		/// </summary>
		protected virtual IFileUtility FileUtility { get; private set; }


		#endregion PRIVATE PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="BaseConfiguration" /> class.
		/// </summary>
		/// <param name="appSettingsReader">The application settings reader.</param>
		/// <param name="configurationManager">The configuration manager.</param>
		/// <param name="fileUtility">The file utility.</param>
		protected BaseConfiguration(
			IAppSettingsReader appSettingsReader,
			IConfigurationManager configurationManager,
			IFileUtility fileUtility)
		{
			AppSettingsReader = appSettingsReader;
			ConfigurationManager = configurationManager;
			FileUtility = fileUtility;
		}


		#endregion CONSTRUCTORS

	}

}
