using System;

namespace MediaFixer.Core.Fixers
{

	/// <summary>
	/// Defines a class that all fixers must implement.
	/// </summary>
	public interface IFixer
	{

		/// <summary>
		/// Fixes the specified directory.
		/// </summary>
		/// <param name="location">The location.</param>
		void FixDirectory(String location);

		/// <summary>
		/// Fixes the specified file.
		/// </summary>
		/// <param name="location">The location.</param>
		void FixFile(String location);

	}

}
