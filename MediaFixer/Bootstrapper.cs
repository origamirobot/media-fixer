using MediaFixer.Core.Configuration;
using MediaFixer.Core.Fixers;
using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
using MediaFixer.Core.Terminal;
using Ninject;

namespace MediaFixer
{

	/// <summary>
	/// Registers the dependencies and bootstraps this application.
	/// </summary>
	static class Bootstrapper
	{

		/// <summary>
		/// Registers all the needed dependencies with the specified Ninject kernel.
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		public static void Register(IKernel kernel)
		{
			kernel.Bind<IAppSettingsReader>().To<CoreAppSettingsReader>();
			kernel.Bind<IConfigurationManager>().To<ConfigManagerWrapper>();
			kernel.Bind<IFileUtility>().To<FileUtility>();
			kernel.Bind<IDirectoryUtility>().To<DirectoryUtility>();
			kernel.Bind<IPathUtility>().To<PathUtility>();
			kernel.Bind<IMediaFixerConfiguration>().To<MediaFixerConfiguration>();
			kernel.Bind<IMovieConfiguration>().To<MovieConfiguration>();
			kernel.Bind<IConsole>().To<ConsoleWrapper>();
			kernel.Bind<IMovieFixer>().To<MovieFixer>();

			var settings = kernel.Get<IMediaFixerConfiguration>();
			log4net.Config.XmlConfigurator.Configure();
			var log4NetLogger = log4net.LogManager.GetLogger(settings.LoggerName);
			kernel.Bind<ILogger>().ToMethod(x => new Log4NetLogger(log4NetLogger, settings)).InSingletonScope();



		}

	}

}
