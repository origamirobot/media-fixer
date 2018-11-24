using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFixer.Core.Terminal
{


	public interface IConsole
	{

		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		void WriteLine(String text);

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		void WriteLine(String text, ConsoleColor color);

		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		void Write(String text);

		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		void Write(String text, ConsoleColor color);

	}

	/// <summary>
	/// Wraps the <see cref="System.Console"/> object.
	/// </summary>
	/// <seealso cref="MediaFixer.Core.Terminal.IConsole" />
	public class ConsoleWrapper : IConsole
	{


		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void Write(String text)
		{
			Console.Write(text);
		}

		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		public void Write(String text, ConsoleColor color)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = originalColor;
		}

		/// <summary>
		/// Writes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void WriteLine(String text)
		{
			Console.WriteLine(text);
		}

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		public void WriteLine(String text, ConsoleColor color)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ForegroundColor = originalColor;
		}

	}

}
