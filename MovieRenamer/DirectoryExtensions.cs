using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRenamer
{
	public static class DirectoryExtensions
	{
		public static void RenameTo(this DirectoryInfo di, string name)
		{
			if (di == null)
				throw new ArgumentNullException("di", "Directory info to rename cannot be null");

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("New name cannot be null or blank", "name");

			di.MoveTo(Path.Combine(di.Parent.FullName, name));

			return; //done
		}

	}

}
