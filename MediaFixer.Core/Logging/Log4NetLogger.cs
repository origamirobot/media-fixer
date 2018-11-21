using MediaFixer.Core.Configuration;
using log4net;
using System;

namespace MediaFixer.Core.Logging
{

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="MediaFixer.Core.Logging.ILogger" />
	public class Log4NetLogger : ILogger
	{

		#region PROTECTED PROPERTIES


		/// <summary>
		/// Gets the logger.
		/// </summary>
		protected ILog Logger { get; private set; }


		/// <summary>
		/// Gets the hyper validator settings.
		/// </summary>
		protected IMediaFixerConfiguration Settings { get; private set; }


		#endregion PROTECTED PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="Log4NetLogger" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="settings">The settings.</param>
		public Log4NetLogger(
			ILog logger, 
			IMediaFixerConfiguration settings)
		{
			Logger = logger;
			Settings = settings;
		}


		#endregion CONSTRUCTORS

		#region PUBLIC METHODS


		/// <summary>
		/// Logs a debug message
		/// </summary>
		/// <param name="message">The message.</param>
		public void Debug(String message)
		{
			if(Settings.LogLevel >= LogLevel.Debug)
				Logger.Debug(message);
		}

		/// <summary>
		/// Logs an info message
		/// </summary>
		/// <param name="message">The message.</param>
		public void Info(String message)
		{
			if (Settings.LogLevel >= LogLevel.Information)
				Logger.Info(message);
		}

		/// <summary>
		/// Logs a warning
		/// </summary>
		/// <param name="message">The message.</param>
		public void Warn(String message)
		{
			if (Settings.LogLevel >= LogLevel.Warning)
				Logger.Warn(message);
		}

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="message">The message.</param>
		public void Error(String message)
		{
			if (Settings.LogLevel >= LogLevel.Error)
				Logger.Error(message);
		}

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="ex">The ex.</param>
		public void Error(Exception ex)
		{
			if (Settings.LogLevel >= LogLevel.Error)
				Logger.Error(ex);
		}


		#endregion PUBLIC METHODS

	}

}
