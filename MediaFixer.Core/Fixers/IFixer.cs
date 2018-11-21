using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFixer.Fixers
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
