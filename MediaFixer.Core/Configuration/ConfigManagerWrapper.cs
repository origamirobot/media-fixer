using System;
using System.Collections.Specialized;
using System.Configuration;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Wraps the <see cref="System.Configuration.ConfigurationManager"/> class because Microsoft is pretty dumb sometimes
	/// and writes untestable code.
	/// </summary>
	public class ConfigManagerWrapper : IConfigurationManager
	{

		/// <summary>
		/// Gets the System.Configuration.AppSettingsSection data for the current application's default configuration.
		/// </summary>
		public NameValueCollection AppSettings
		{
			get { return ConfigurationManager.AppSettings; }
		}

		/// <summary>
		/// Gets the System.Configuration.ConnectionStringsSection data for the current application's default configuration.
		/// </summary>
		public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionName">The configuration section path and name.</param>
		/// <returns>
		/// The specified System.Configuration.ConfigurationSection object, or null if
		/// the section does not exist.
		/// </returns>
		public Object GetSection(String sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}

		/// <summary>
		/// Opens the configuration file for the current application as a System.Configuration.Configuration object.
		/// </summary>
		/// <param name="userLevel">The System.Configuration.ConfigurationUserLevel for which you are opening the configuration.</param>
		/// <returns>
		/// A System.Configuration.Configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
		{
			return ConfigurationManager.OpenExeConfiguration(userLevel);
		}

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration object.
		/// </summary>
		/// <param name="exePath">The path of the executable (exe) file.</param>
		/// <returns>
		/// A System.Configuration.Configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenExeConfiguration(String exePath)
		{
			return ConfigurationManager.OpenExeConfiguration(exePath);
		}

		/// <summary>
		/// Opens the machine configuration file on the current computer as a System.Configuration.Configuration object.
		/// </summary>
		/// <returns>
		/// A System.Configuration.Configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenMachineConfiguration()
		{
			return ConfigurationManager.OpenMachineConfiguration();
		}

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration object that uses the specified file mapping and user level.
		/// </summary>
		/// <param name="fileMap">An System.Configuration.ExeConfigurationFileMap object that references configuration file to use instead of the application default configuration file.</param>
		/// <param name="userLevel">The System.Configuration.ConfigurationUserLevel object for which you are opening the configuration.</param>
		/// <returns>
		/// The configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
		{
			return ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel);
		}

		/// <summary>
		/// Opens the specified client configuration file as a System.Configuration.Configuration
		/// object that uses the specified file mapping, user level, and preload option.
		/// </summary>
		/// <param name="fileMap">An System.Configuration.ExeConfigurationFileMap object that references the
		/// configuration file to use instead of the default application configuration
		/// file.</param>
		/// <param name="userLevel">The System.Configuration.ConfigurationUserLevel object for which you are
		/// opening the configuration.</param>
		/// <param name="preLoad">true to preload all section groups and sections; otherwise, false.</param>
		/// <returns>
		/// The configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, Boolean preLoad)
		{
			return ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel, preLoad);
		}

		/// <summary>
		/// Opens the machine configuration file as a System.Configuration.Configuration object that uses the specified file mapping.
		/// </summary>
		/// <param name="fileMap">An System.Configuration.ExeConfigurationFileMap object that references configuration
		/// file to use instead of the application default configuration file.</param>
		/// <returns>
		/// A System.Configuration.Configuration object.
		/// </returns>
		public System.Configuration.Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap)
		{
			return ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
		}

		/// <summary>
		/// Refreshes the named section so the next time that it is retrieved it will be re-read from disk.
		/// </summary>
		/// <param name="sectionName">The configuration section name or the configuration path and section name of the section to refresh.</param>
		public void RefreshSection(String sectionName)
		{
			ConfigurationManager.RefreshSection(sectionName);
		}

	}

}
