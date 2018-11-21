using System;
using MediaFixer.Core.Logging;

namespace MediaFixer.Core.Configuration
{

	/// <summary>
	/// 
	/// </summary>
	public interface IMediaFixerConfiguration
	{

		/// <summary>
		/// Gets the log level.
		/// </summary>
		LogLevel LogLevel { get; }

		/// <summary>
		/// Gets the name of the logger.
		/// </summary>
		String LoggerName { get; }

	}

}
