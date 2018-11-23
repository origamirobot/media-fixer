using System;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Defines a contract that all application settings readers must implement.
	/// </summary>
	/// <remarks>
	/// This interface can be mocked for unit testing configuration settings.
	/// </remarks>
	public interface IAppSettingsReader
	{

		#region PUBLIC METHODS


		/// <summary>
		/// Gets the value of the specified key and type
		/// </summary>
		/// <param name="key">The key to get the value for.</param>
		/// <param name="type">The type of the value to get.</param>
		/// <returns>Returns the value of the specified key</returns>
		Object GetValue(String key, Type type);

		/// <summary>
		/// Determines whether the specified key is a setting in the app/web.config file.
		/// </summary>
		/// <param name="key">The key to check for.</param>
		/// <returns><c>true</c> if the specified key is in the app/web.config file; otherwise <c>false</c></returns>
		Boolean HasSetting(String key);

		/// <summary>
		/// Reads the specified app setting from any configuration file.
		/// </summary>
		/// <param name="key">The key to read from.</param>
		/// <returns>Returns the value of the specified app setting</returns>
		String ReadAppSetting(String key);

		/// <summary>
		/// Writes the application settings.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		Boolean WriteAppSetting(String key, String value);

		/// <summary>
		/// Gets the specified configuration section.
		/// </summary>
		/// <typeparam name="T">The configuration section type</typeparam>
		/// <param name="section">The section to get.</param>
		/// <returns>Returns the specified configuration section</returns>
		T GetConfigSection<T>(String section) where T : class;



		#endregion PUBLIC METHODS

		#region STRING METHODS


		/// <summary>
		/// Gets the string value for a specified key from a specific config file.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The config file to read the setting from.</param>
		/// <returns>Returns the string value of the specified key from the specified config file.</returns>
		String ReadStringAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the app setting directly from the web.config file.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <returns>Returns the string value of the app setting from the default config store.</returns>
		String ReadStringAppSettingFromDefaultStore(String key);

		/// <summary>
		/// Gets the string value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		String ReadOptionalStringAppSetting(String key, String defaultValue);

		/// <summary>
		/// Gets the string value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The config file to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the string value of the specified key from the specified config file.</returns>
		String ReadOptionalStringAppSetting(String key, String configFile, String defaultValue);


		#endregion STRING METHODS

		#region BOOLEAN METHODS


		/// <summary>
		/// Reads the boolean value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the boolean value from.</param>
		/// <param name="configFile">The configuration file to read the boolean value from.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Boolean ReadBooleanAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the boolean value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the boolean value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		Boolean ReadBooleanAppSettingFromDefaultStore(String key);

		/// <summary>
		/// Gets the boolean value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Boolean ReadOptionalBooleanAppSetting(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the boolean value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the boolean value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Boolean ReadOptionalBooleanAppSetting(String key, String configFile, Boolean defaultValue);


		#endregion BOOLEAN METHODS

		#region INTEGER METHODS


		/// <summary>
		/// Reads the int value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the int value from.</param>
		/// <param name="configFile">The configuration file to read the int value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		Int32 ReadIntAppSetting(String key, String configFile);

		/// <summary>
		/// Gets the int value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Int32 ReadOptionalIntAppSetting(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the int value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the int value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Int32 ReadOptionalIntAppSetting(String key, String configFile, Int32 defaultValue);

		/// <summary>
		/// Reads the Int32 value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Int32 value from  + key +  in appSettings.</exception>
		Int32 ReadIntAppSettingFromDefaultStore(String key);


		#endregion INTEGER METHODS


		#region INTEGER METHODS


		/// <summary>
		/// Reads the Int64 value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the int value from.</param>
		/// <param name="configFile">The configuration file to read the int value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		Int64 ReadInt64AppSetting(String key, String configFile);

		/// <summary>
		/// Gets the Int64 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Int64 ReadOptionalInt64AppSetting(String key, Int64 defaultValue);

		/// <summary>
		/// Gets the Int64 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the int value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Int64 ReadOptionalInt64AppSetting(String key, String configFile, Int64 defaultValue);

		/// <summary>
		/// Reads the Int64 value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Int32 value from  + key +  in appSettings.</exception>
		Int64 ReadInt64AppSettingFromDefaultStore(String key);


		#endregion INTEGER METHODS


		#region DOUBLE METHODS


		/// <summary>
		/// Reads the double value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the double value from.</param>
		/// <param name="configFile">The configuration file to read the double value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		Double ReadDoubleAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the double value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the double value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		Double ReadDoubleAppSettingFromDefaultStore(String key);

		/// <summary>
		/// Gets the double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Double ReadOptionalDoubleAppSetting(String key, Double defaultValue);

		/// <summary>
		/// Gets the double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the double value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Double ReadOptionalDoubleAppSetting(String key, String configFile, Double defaultValue);


		#endregion DOUBLE METHODS

		#region SINGLE METHODS


		/// <summary>
		/// Reads the Single value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Single value from.</param>
		/// <param name="configFile">The configuration file to read the Single value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		Single ReadSingleAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the Single value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Single value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		Single ReadSingleAppSettingFromDefaultStore(String key);

		/// <summary>
		/// Gets the Single value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Single ReadOptionalSingleAppSetting(String key, Single defaultValue);

		/// <summary>
		/// Gets the Single value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the Single value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Single ReadOptionalSingleAppSetting(String key, String configFile, Single defaultValue);


		#endregion SINGLE METHODS

		#region DECIMAL METHODS


		/// <summary>
		/// Reads the decimal value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the decimal value from.</param>
		/// <param name="configFile">The configuration file to read the decimal value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		Decimal ReadDecimalAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the decimal value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the decimal value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		Decimal ReadDecimalAppSettingFromDefaultStore(String key);

		/// <summary>
		/// Gets the decimal value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		Decimal ReadOptionalDecimalAppSetting(String key, Decimal defaultValue);

		/// <summary>
		/// Gets the decimal value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the decimal value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Decimal ReadOptionalDecimalAppSetting(String key, String configFile, Decimal defaultValue);


		#endregion DECIMAL METHODS

		#region ENUM METHODS


		/// <summary>
		/// Reads the value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the decimal value from.</param>
		/// <param name="configFile">The configuration file to read the decimal value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		T ReadEnumAppSetting<T>(String key, String configFile) where T : struct, IConvertible;

		/// <summary>
		/// Reads the value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the decimal value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		T ReadEnumAppSettingFromDefaultStore<T>(String key) where T : struct, IConvertible;

		/// <summary>
		/// Gets the value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		T ReadOptionalEnumAppSetting<T>(String key, T defaultValue) where T : struct, IConvertible;

		/// <summary>
		/// Gets the value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the decimal value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		T ReadOptionalEnumAppSetting<T>(String key, String configFile, T defaultValue) where T : struct, IConvertible;


		#endregion ENUM METHODS

		#region INTEGER ARRAY METHODS


		/// <summary>
		/// Gets the Int32 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		Int32[] ReadOptionalIntArrayAppSetting(String key, params Int32[] defaultValue);

		/// <summary>
		/// Reads the Int32 value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the specified configuration file
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		Int32[] ReadIntArrayAppSetting(String key, String configFile);

		/// <summary>
		/// Reads the Int32 value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Int32 value from  + key +  in appSettings.</exception>
		Int32[] ReadIntArrayAppSettingFromDefaultStore(String key);

		#endregion

	}

}
