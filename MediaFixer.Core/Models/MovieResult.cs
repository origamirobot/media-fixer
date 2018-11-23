using System;

namespace MediaFixer.Core.Models
{

	/// <summary>
	/// 
	/// </summary>
	public class MovieResult
	{

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		public String Title { get; set; }

		/// <summary>
		/// Gets or sets the year.
		/// </summary>
		public Int32? Year { get; set; }

		/// <summary>
		/// Gets or sets the original path of this movie.
		/// </summary>
		public String Path { get; set; }

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		public Int64 Length { get; set; }



	}

}
