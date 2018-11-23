using System;
using System.Runtime.Serialization;

namespace MediaFixer.Core
{


	/// <summary>
	/// An exception that is thrown when a movie file cannot be found with-in a given directory.
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class MovieNotFoundException : Exception
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
		/// </summary>
		public MovieNotFoundException() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public MovieNotFoundException(String message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		public MovieNotFoundException(String message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		protected MovieNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}

}
