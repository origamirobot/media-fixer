using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// Default application settings reader for Olympus based projects.
	/// </summary>
	public class CoreAppSettingsReader : AppSettingsReader, IAppSettingsReader
	{

		#region PROTECTED PROPERTIES


		/// <summary>
		/// Gets the configuration manager.
		/// </summary>
		protected IConfigurationManager ConfigurationManager { get; private set; }


		#endregion PROTECTED PROPERTIES

		#region PUBLIC PROPERTIES


		/// <summary>
		/// Gets or sets the configuration sections for this application.
		/// </summary>
		public IEnumerable<IKeyValueSection> ConfigSections { get; set; }


		#endregion PUBLIC PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="CoreAppSettingsReader" /> class.
		/// </summary>
		/// <param name="configurationManager">The configuration manager.</param>
		/// <exception cref="System.ApplicationException">Error reading the IKeyValueSection  + sectionName +  in app settings.</exception>
		public CoreAppSettingsReader(
			IConfigurationManager configurationManager) : this(configurationManager, new String[0]) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CoreAppSettingsReader" /> class.
		/// </summary>
		/// <param name="configurationManager">The configuration manager.</param>
		/// <param name="orderedSectionNames">The ordered section names.</param>
		/// <exception cref="System.ApplicationException">Error reading the IKeyValueSection  + sectionName +  in app settings.</exception>
		public CoreAppSettingsReader(
			IConfigurationManager configurationManager,
			params String[] orderedSectionNames)
		{
			ConfigurationManager = configurationManager;
			// FIND ALL OF THE CONFIGURATION SECTIONS AND ADD THEM TO PROPERTY.
			var configSections = new Collection<IKeyValueSection>();
			foreach (var sectionName in orderedSectionNames)
			{
				try
				{
					configSections.Add(GetConfigSection<IKeyValueSection>(sectionName));
				}
				catch (Exception ex)
				{
					throw new ApplicationException("Error reading the IKeyValueSection " + sectionName + " in app settings.", ex);
				}
			}
			ConfigSections = configSections;
		}


		#endregion CONSTRUCTORS

		#region IAppSettingsReader MEMEBERS


		/// <summary>
		/// Gets the specified configuration section.
		/// </summary>
		/// <typeparam name="T">The configuration section type</typeparam>
		/// <param name="section">The section to get.</param>
		/// <returns>
		/// Returns the specified configuration section
		/// </returns>
		public T GetConfigSection<T>(String section) where T : class
		{
			return ConfigurationManager.GetSection(section) as T;
		}

		/// <summary>
		/// Reads the specified app setting from any configuration file.
		/// </summary>
		/// <param name="key">The key to read from.</param>
		/// <returns></returns>
		public String ReadAppSetting(String key)
		{
			foreach (var section in ConfigSections)
			{
				if (section == null)
					continue;
				var val = section.Elements.GetValue(key);
				if (val != null)
					return val;
			}
			return ReadStringAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Determines whether the specified key is a setting in the app/web.config file.
		/// </summary>
		/// <param name="key">The key to check for.</param>
		/// <returns>
		///   <c>true</c> if the specified key is in the app/web.config file; otherwise <c>false</c>
		/// </returns>
		public Boolean HasSetting(String key)
		{
			try
			{
				GetValue(key, typeof(Object));
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Writes the application setting.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <see href="http://stackoverflow.com/a/11841175/1398981"/>
		public Boolean WriteAppSetting(String key, String value)
		{
			var result = false;
			try
			{
				System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				config.AppSettings.Settings.Remove(key);
				var kvElem = new KeyValueConfigurationElement(key, value);
				config.AppSettings.Settings.Add(kvElem);
				// Save the configuration file.
				config.Save(ConfigurationSaveMode.Modified);

				// Force a reload of a changed section.
				ConfigurationManager.RefreshSection("appSettings");

				result = true;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString()); 
			}
			finally { } 
			return result;
		}

		

		#region STRING SETTINGS READERS


		/// <summary>
		/// Gets the String value for a specified key from a specific config file
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The config file to read the setting from.</param>
		/// <returns>
		/// Returns the String value of the specified key from the specified config file.
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public String ReadStringAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					return config.Elements.GetValue(key);
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the String value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadStringAppSettingFromDefaultStore(key);

		}

		/// <summary>
		/// Reads the app setting directly from the web.config file.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <returns>
		/// Returns the String value of the app setting from the default config store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the String value from  + key +  in appSettings.</exception>
		public String ReadStringAppSettingFromDefaultStore(String key)
		{
			try
			{
				return GetValue(key, typeof(String)) as String;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the String value from " + key + " in appSettings.", ex);
			}
		}

		/// <summary>
		/// Gets the String value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public String ReadOptionalStringAppSetting(String key, String defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
					return keyValue;
			}
			catch { }

			return defaultValue;
		}

		/// <summary>
		/// Gets the String value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The config file to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public String ReadOptionalStringAppSetting(String key, String configFile, String defaultValue)
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
					return config.Elements.GetValue(key);
			}
			catch { }
			// FALLBACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion STRING SETTINGS READERS

		#region BOOLEAN SETTINGS READERS


		/// <summary>
		/// Reads the boolean value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the boolean value from.</param>
		/// <param name="configFile">The configuration file to read the boolean value from.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Boolean ReadBooleanAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					Boolean val;
					if (Boolean.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the boolean value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadBooleanAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the boolean value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the boolean value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the boolean value from  + key +  in appSettings.</exception>
		public Boolean ReadBooleanAppSettingFromDefaultStore(String key)
		{
			try
			{
				Boolean val;
				if (Boolean.TryParse(GetValue(key, typeof(String)) as String, out val))
					return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the boolean value from " + key + " in appSettings.", ex);
			}
			throw new ApplicationException("Error reading the boolean value from " + key + " in appSettings.");
		}

		/// <summary>
		/// Gets the boolean value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Boolean ReadOptionalBooleanAppSetting(String key, Boolean defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Boolean val;
					if (Boolean.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Gets the boolean value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the boolean value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Boolean ReadOptionalBooleanAppSetting(String key, String configFile, Boolean defaultValue)
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
				{
					Boolean val;
					if (Boolean.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
			}
			catch { }
			// FALLBACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion BOOLEAN SETTINGS READERS

		#region INTEGER SETTINGS READERS


		/// <summary>
		/// Gets the Int32 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Int32 ReadOptionalIntAppSetting(String key, Int32 defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Int32 val;
					if (Int32.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Gets the Int32 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Int32 ReadOptionalIntAppSetting(String key, String configFile, Int32 defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Int32 val;
					if (Int32.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Reads the Int32 value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the specified configuration file
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Int32 ReadIntAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					Int32 val;
					if (Int32.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Int32 value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadIntAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the Int32 value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Int32 value from  + key +  in appSettings.</exception>
		public Int32 ReadIntAppSettingFromDefaultStore(String key)
		{
			try
			{
				Int32 val;
				if (Int32.TryParse(GetValue(key, typeof(String)) as String, out val))
					return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the Int32 value from " + key + " in appSettings.", ex);
			}
			throw new ApplicationException("Error reading the Int32 value from " + key + " in appSettings.");
		}


		#endregion INTEGER SETTINGS READERS

		#region INTEGER ARRAY SETTINGS READERS


		/// <summary>
		/// Gets the Int32 value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Int32[] ReadOptionalIntArrayAppSetting(String key, params Int32[] defaultValue)
		{
			try
			{
				var keyValue = ReadAppSetting(key);
				var list = new List<Int32>();
				if (keyValue != null)
				{
					var split = keyValue.Split(',');
					foreach(var item in split)
					{
						var val = 0;
						if (Int32.TryParse(item, out val))
							list.Add(val);
					}
					return list.ToArray();
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Reads the Int32 value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the specified configuration file
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Int32[] ReadIntArrayAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					var list = new List<Int32>();
					var val = config.Elements.GetValue(key);
					var split = val.Split(',');
					foreach (var item in split)
					{
						var i = 0;
						if (Int32.TryParse(item, out i))
							list.Add(i);
					}
					return list.ToArray();
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Int32 value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadIntArrayAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the Int32 value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Int32 value from  + key +  in appSettings.</exception>
		public Int32[] ReadIntArrayAppSettingFromDefaultStore(String key)
		{
			try
			{
				var list = new List<Int32>();
				var val = GetValue(key, typeof(String)) as String;
				var split = val.Split(',');
				foreach(var item in split)
				{
					var i = 0;
					if (Int32.TryParse(item, out i))
						list.Add(i);
				}
				return list.ToArray();
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the Int32 array value from " + key + " in appSettings.", ex);
			}
		}


		#endregion INTEGER ARRAY SETTINGS READERS

		#region DOUBLE SETTINGS READERS


		/// <summary>
		/// Reads the Double value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Double value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the specified configuration file
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Double ReadDoubleAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					Double val;
					if (Double.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Double value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadIntAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the Double value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Double value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Double value from  + key +  in appSettings.</exception>
		public Double ReadDoubleAppSettingFromDefaultStore(String key)
		{
			try
			{
				Double val;
				if (Double.TryParse(GetValue(key, typeof(String)) as String, out val))
					return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the Double value from " + key + " in appSettings.", ex);
			}
			throw new ApplicationException("Error reading the Double value from " + key + " in appSettings.");
		}

		/// <summary>
		/// Gets the Double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Double ReadOptionalDoubleAppSetting(String key, Double defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Double val;
					if (Double.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Gets the Double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the Double value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Double ReadOptionalDoubleAppSetting(String key, String configFile, Double defaultValue)
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
				{
					Double val;
					if (Double.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
			}
			catch { }
			// FALL BACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion DOUBLE SETTINGS READERS

		#region SINGLE SETTINGS READERS


		/// <summary>
		/// Reads the single value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Double value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns></returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Single ReadSingleAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					Single val;
					if (Single.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Double value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadIntAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the single value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the single value from.</param>
		/// <returns></returns>
		/// <exception cref="System.ApplicationException">Error reading the single value from  + key +  in appSettings.</exception>
		public Single ReadSingleAppSettingFromDefaultStore(String key)
		{
			try
			{
				Single val;
				if (Single.TryParse(GetValue(key, typeof(String)) as String, out val))
					return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the single value from " + key + " in appSettings.", ex);
			}
			throw new ApplicationException("Error reading the single value from " + key + " in appSettings.");
		}

		/// <summary>
		/// Gets the single value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns></returns>
		public Single ReadOptionalSingleAppSetting(String key, Single defaultValue)
		{
			try
			{
				var keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Single val;
					if (Single.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Gets the single value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the single value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Single ReadOptionalSingleAppSetting(String key, String configFile, Single defaultValue)
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
				{
					Single val;
					if (Single.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
			}
			catch { }
			// FALL BACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion SINGLE SETTINGS READERS


		#region DECIMAL SETTINGS READERS


		/// <summary>
		/// Reads the Double value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Double value from.</param>
		/// <param name="configFile">The configuration file to read the Int32 value from.</param>
		/// <returns>
		/// Returns the value of the key from the specified configuration file
		/// </returns>
		/// <exception cref="System.ApplicationException"></exception>
		public Decimal ReadDecimalAppSetting(String key, String configFile)
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					Decimal val;
					if (Decimal.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Decimal value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			return ReadDecimalAppSettingFromDefaultStore(key);
		}

		/// <summary>
		/// Reads the Double value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Double value from.</param>
		/// <returns>
		/// Returns the value of the key from the default configuration store.
		/// </returns>
		/// <exception cref="System.ApplicationException">Error reading the Double value from  + key +  in appSettings.</exception>
		public Decimal ReadDecimalAppSettingFromDefaultStore(String key)
		{
			try
			{
				Decimal val;
				if (Decimal.TryParse(GetValue(key, typeof(String)) as String, out val))
					return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the Decimal value from " + key + " in appSettings.", ex);
			}
			throw new ApplicationException("Error reading the Decimal value from " + key + " in appSettings.");
		}

		/// <summary>
		/// Gets the Double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Decimal ReadOptionalDecimalAppSetting(String key, Decimal defaultValue)
		{
			try
			{
				String keyValue = ReadAppSetting(key);
				if (keyValue != null)
				{
					Decimal val;
					if (Decimal.TryParse(keyValue, out val))
						return val;
				}
			}
			catch { }
			return defaultValue;
		}

		/// <summary>
		/// Gets the Double value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the Decimal value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public Decimal ReadOptionalDecimalAppSetting(String key, String configFile, Decimal defaultValue)
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
				{
					Decimal val;
					if (Decimal.TryParse(config.Elements.GetValue(key), out val))
						return val;
				}
			}
			catch { }
			// FALL BACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion DECIMAL SETTINGS READERS

		#region ENUM SETTINGS READERS


		/// <summary>
		/// Reads the value for the specified key from the specified config file
		/// </summary>
		/// <param name="key">The key to read the Decimal value from.</param>
		/// <param name="configFile">The configuration file to read the Decimal value from.</param>
		/// <returns>Returns the value of the key from the specified configuration file</returns>
		public T ReadEnumAppSetting<T>(String key, String configFile) where T : struct, IConvertible
		{
			// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
			var config = GetConfigSection<IKeyValueSection>(configFile);
			if (config != null)
			{
				try
				{
					var keyValue = config.Elements.GetValue(key);
					T val;
					if (Enum.TryParse<T>(keyValue, out val))
						return val;
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Error reading the Enum value from {key} in {configFile} config file.", ex);
				}
			}
			// FALLBACK TO NORMAL WEB.CONFIG IF AVAILABLE
			var defaultKeyValue = ReadStringAppSettingFromDefaultStore(key);
			return (T)Enum.Parse(typeof(T), defaultKeyValue);
		}

		/// <summary>
		/// Reads the value for the specified key directly from the web.config file
		/// </summary>
		/// <param name="key">The key to read the Decimal value from.</param>
		/// <returns>Returns the value of the key from the default configuration store.</returns>
		public T ReadEnumAppSettingFromDefaultStore<T>(String key) where T : struct, IConvertible
		{
			try
			{
				var keyValue = GetValue(key, typeof(String)) as String;
				T val;
				if (!Enum.TryParse<T>(keyValue, out val))
					throw new Exception();

				return val;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error reading the Enum value from " + key + " in appSettings.", ex);
			}
		}

		/// <summary>
		/// Gets the value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>Returns the value of the key if present; otherwise, the default value</returns>
		public T ReadOptionalEnumAppSetting<T>(String key, T defaultValue) where T : struct, IConvertible
		{
			try
			{
				var keyValue = ReadAppSetting(key);
				T val;
				if (Enum.TryParse<T>(keyValue, out val))
					return val;
			}
			catch { }

			return defaultValue;
		}

		/// <summary>
		/// Gets the value for a specified key. If the key is not present in the config; returns <paramref name="defaultValue" />.
		/// </summary>
		/// <param name="key">The key to read the setting from.</param>
		/// <param name="configFile">The configuration file to read the Decimal value from.</param>
		/// <param name="defaultValue">The default value to return if key isn't present.</param>
		/// <returns>
		/// Returns the value of the key if present; otherwise, the default value
		/// </returns>
		public T ReadOptionalEnumAppSetting<T>(String key, String configFile, T defaultValue) where T : struct, IConvertible
		{
			try
			{
				// TRY TO READ VALUE FROM SPECIFIED CONFIGURATION FILE
				var config = GetConfigSection<IKeyValueSection>(configFile);
				if (config != null)
				{
					T val;
					if (Enum.TryParse<T>(config.Elements.GetValue(key), out val))
						return val;
				}
			}
			catch { }
			// FALLBACK TO DEFAULT VALUE PROVIDED
			return defaultValue;
		}


		#endregion ENUM SETTINGS READERS


		#endregion IAppSettingsReader MEMEBERS

	}

}
