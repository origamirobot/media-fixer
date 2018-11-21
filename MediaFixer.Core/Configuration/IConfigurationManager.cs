using System;
using System.Collections.Specialized;
using System.Configuration;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Acts as a wrapper around the <see cref="System.Configuration.ConfigurationManager"/> class.
	/// </summary>
	public interface IConfigurationManager
	{

		/// <summary>
		/// Gets the System.Configuration.AppSettingsSection data for the current application's default configuration.
		/// </summary>
		NameValueCollection AppSettings { get; }

		/// <summary>
		/// Gets the System.Configuration.ConnectionStringsSection data for the current application's default configuration.
		/// </summary>
		ConnectionStringSettingsCollection ConnectionStrings { get; }

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionName">The configuration section path and name.</param>
		/// <returns>
		/// The specified System.Configuration.ConfigurationSection object, or null if
		/// the section does not exist.
		/// </returns>
		Object GetSection(String sectionName);

		/// <summary>
		/// Opens the configuration file for the current application as a System.Configuration.Configuration object.
		/// </summary>
		/// <param name="userLevel">The System.Configuration.ConfigurationUserLevel for which you are opening the configuration.</param>
		/// <returns>A System.Configuration.Configuration object.</returns>
		System.Configuration.Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel);

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration object.
		/// </summary>
		/// <param name="exePath"> The path of the executable (exe) file.</param>
		/// <returns>A System.Configuration.Configuration object.</returns>
		System.Configuration.Configuration OpenExeConfiguration(String exePath);

		/// <summary>
		/// Opens the machine configuration file on the current computer as a System.Configuration.Configuration object.
		/// </summary>
		/// <returns>A System.Configuration.Configuration object.</returns>
		System.Configuration.Configuration OpenMachineConfiguration();

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration object that uses the specified file mapping and user level.
		/// </summary>
		/// <param name="fileMap">An System.Configuration.ExeConfigurationFileMap object that references configuration file to use instead of the application default configuration file.</param>
		/// <param name="userLevel">The System.Configuration.ConfigurationUserLevel object for which you are opening the configuration.</param>
		/// <returns>The configuration object.</returns>
		System.Configuration.Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel);

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration
		/// object that uses the specified file mapping, user level, and preload option.
		/// </summary>
		/// <param name="fileMap">
		/// An System.Configuration.ExeConfigurationFileMap object that references the
		/// configuration file to use instead of the default application configuration
		/// file.
		/// </param>
		/// <param name="userLevel">
		/// The System.Configuration.ConfigurationUserLevel object for which you are
		/// opening the configuration.
		/// </param>
		/// <param name="preLoad">true to preload all section groups and sections; otherwise, false.</param>
		/// <returns>The configuration object.</returns>
		System.Configuration.Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, Boolean preLoad);

		/// <summary>
		/// Opens the machine configuration file as a System.Configuration.Configuration object that uses the specified file mapping.
		/// </summary>
		/// <param name="fileMap">
		/// An System.Configuration.ExeConfigurationFileMap object that references configuration 
		/// file to use instead of the application default configuration file.
		/// </param>
		/// <returns>A System.Configuration.Configuration object.</returns>
		System.Configuration.Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap);

		/// <summary>
		/// Refreshes the named section so the next time that it is retrieved it will be re-read from disk.
		/// </summary>
		/// <param name="sectionName">The configuration section name or the configuration path and section name of the section to refresh.</param>
		void RefreshSection(String sectionName);

	}

}
