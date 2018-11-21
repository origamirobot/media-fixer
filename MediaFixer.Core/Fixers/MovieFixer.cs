using MediaFixer.Core.Configuration;
using System;

namespace MediaFixer.Core.Fixers
{

	/// <summary>
	/// Defines a contract that all movie fixers must implement.
	/// </summary>
	/// <seealso cref="MediaFixer.Fixers.IFixer" />
	public interface IMovieFixer : IFixer
	{

	}


	/// <summary>
	/// Responsible for fixing movies.
	/// </summary>
	public class MovieFixer : IMovieFixer
	{

		#region PROTECTED PROPERTIES


		/// <summary>
		/// Gets the settings.
		/// </summary>
		protected IMovieConfiguration Settings { get; private set; }


		#endregion PROTECTED PROPERTIES

		#region CONSTRUCTORS


		/// <summary>
		/// Initializes a new instance of the <see cref="MovieFixer"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public MovieFixer(IMovieConfiguration settings)
		{
			Settings = settings;
		}


		#endregion CONSTRUCTORS

		#region PUBLIC METHODS


		/// <summary>
		/// Fixes the directory.
		/// </summary>
		/// <param name="location">The location.</param>
		public void FixDirectory(String location)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fixes the file.
		/// </summary>
		/// <param name="location">The location.</param>
		public void FixFile(String location)
		{
			throw new NotImplementedException();
		}


		#endregion PUBLIC METHODS


	}


}
