using System;

namespace MediaFixer.Core.Logging
{

	/// <summary>
	/// 
	/// </summary>
	public interface ILogger
	{

		/// <summary>
		/// Logs a debug message
		/// </summary>
		/// <param name="message">The message.</param>
		void Debug(String message);
		
		/// <summary>
		/// Logs an info message
		/// </summary>
		/// <param name="message">The message.</param>
		void Info(String message);
		
		/// <summary>
		/// Logs a warning
		/// </summary>
		/// <param name="message">The message.</param>
		void Warn(String message);

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="message">The message.</param>
		void Error(String message);

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="ex">The ex.</param>
		void Error(Exception ex);

	}

}
