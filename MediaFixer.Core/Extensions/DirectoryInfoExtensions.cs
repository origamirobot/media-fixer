using System;
using System.IO;

namespace MediaFixer.Core.Extensions
{

	/// <summary>
	/// Extension methods for <see cref="DirectoryInfo"/>.
	/// </summary>
	public static class DirectoryInfoExtensions
	{

		/// <summary>
		/// Renames this directory to something else.
		/// </summary>
		/// <param name="di">The di.</param>
		/// <param name="name">The name.</param>
		/// <exception cref="System.ArgumentNullException">di - Directory info to rename cannot be null</exception>
		/// <exception cref="System.ArgumentException">New name cannot be null or blank - name</exception>
		public static void RenameTo(this DirectoryInfo di, String name)
		{
			if (di == null)
				throw new ArgumentNullException(nameof(di), "Directory info to rename cannot be null");

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("New name cannot be null or blank", nameof(name));

			if (di.Parent != null)
				di.MoveTo(Path.Combine(di.Parent.FullName, name));

		}

	}

}
