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
		void Fix(String location);

	}

}
