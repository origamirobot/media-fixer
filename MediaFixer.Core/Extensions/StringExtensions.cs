using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaFixer.Core.Extensions
{

	/// <summary>
	/// Extension method
	/// </summary>
	public static class StringExtensions
	{

		#region PRIVATE CONSTANTS


		/// <summary>
		/// Regular expression used for stripping HTML from text.
		/// </summary>
		public const String HtmlRegEx = "<(.|\n)+?>";


		#endregion PRIVATE CONSTANTS

		#region PRIVATE METHODS



		/// <summary>
		/// Tests the regex pattern against the provided string.
		/// </summary>
		/// <param name="text">The string to test</param>
		/// <param name="pattern">The regex pattern to test with</param>
		/// <returns></returns>
		private static Boolean TestPattern(String text, String pattern)
		{
			if (String.IsNullOrEmpty(text))
				return false;

			var regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.IsMatch(text);
		}


		#endregion PRIVATE METHODS

		#region EXTENSION METHODS


		/// <summary>
		/// Removes the query string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String RemoveQueryString(this String value)
		{
			var index = value.IndexOf('?');
			return index < 0 ? value : value.Substring(0, index);
		}

		/// <summary>
		/// Returns a string array that contains the substrings in this string that are delimited by elements of a specified string array. 
		/// A parameter specifies whether to return empty array elements.
		/// </summary>
		/// <param name="value">The string this method is extending.</param>
		/// <param name="separator">A string that delimit the substrings in this string.</param>
		/// <param name="options">
		/// System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; 
		/// or System.StringSplitOptions.None to include empty array elements in the array returned.
		/// </param>
		/// <returns>
		/// An array whose elements contain the substrings in this string that are delimited 
		/// by one or more strings in separator. For more information, see the Remarks section.
		/// </returns>
		public static String[] Split(this String value, String separator, StringSplitOptions options)
		{
			return value.Split(new[] { separator }, options);
		}

		/// <summary>
		/// Limits the amount of text displayed regardless of word barriers.
		/// </summary>
		/// <param name="value">The string this method is extending.</param>
		/// <param name="characterCount">The amount of characters to show.</param>
		/// <returns></returns>
		public static String Limit(this String value, Int32 characterCount)
		{
			return value.Length <= characterCount
				? value
				: value.Substring(0, characterCount).TrimEnd(' ');
		}

		/// <summary>
		/// Limits the amount of text displayed regardless of word barriers while adding an ellipses at the end.
		/// </summary>
		/// <param name="value">The string this method is extending.</param>
		/// <param name="characterCount">The amount of characters to show.</param>
		/// <returns>A version of this string truncated to the specified size.</returns>
		public static String LimitWithEllipses(this String value, Int32 characterCount)
		{
			if (characterCount < 5)
				return value.Limit(characterCount);

			if (value.Length <= characterCount - 3)
				return value;

			return value.Substring(0, characterCount - 3) + "…";
		}

		/// <summary>
		/// Limits the amount of text displayed. If <paramref name="breakOnWordBarrier"/> is set to <c>true</c>; truncation will not break in the middle of words.
		/// </summary>
		/// <param name="value">The string this method is extending.</param>
		/// <param name="characterCount">The amount of characters to show.</param>
		/// <param name="breakOnWordBarrier">if set to <c>true</c> the string wont truncate in the middle of a word.</param>
		/// <returns>A version of this string truncated to the specified size.</returns>
		public static String LimitWithEllipses(this String value, Int32 characterCount, Boolean breakOnWordBarrier)
		{
			if (!breakOnWordBarrier)
				return LimitWithEllipses(value, characterCount);

			if (characterCount < 5)
				return value.Limit(characterCount);

			if (value.Length <= characterCount - 3)
				return value;

			var lastspace = value.Substring(0, characterCount - 3).LastIndexOf(' ');
			if (lastspace > 0 && lastspace > characterCount - 10)
				return value.Substring(0, lastspace) + "…";

			// No suitable space was found
			return value.Substring(0, characterCount - 3) + "…";
		}

		/// <summary>
		/// Converts this string to a URL friendly version. This method can be used for making URL slugs
		/// </summary>
		/// <param name="s">The string this method is extending.</param>
		/// <returns></returns>
		public static String UrlFriendly(this String s)
		{
			if (String.IsNullOrEmpty(s))
				return " ";
			s = s.ToLower().Replace(" ", "-").Replace("/", "");
			s = s.Replace("&", "and");
			s = s.Replace(".", "");
			s = Regex.Replace(s, @"[^\w\.@-]", "");
			return s;
		}

		/// <summary>
		/// Strips the HTML.
		/// </summary>
		/// <param name="value">The string instance that this method extends.</param>
		/// <returns></returns>
		public static String StripHtml(this String value)
		{
			var objRegExp = new Regex(HtmlRegEx, RegexOptions.Singleline);
			var strOutput = objRegExp.Replace(value, String.Empty);
			return strOutput;
		}

		/// <summary>
		/// Obscures the text specified.
		/// </summary>
		/// <param name="value">The string instance that this method extends.</param>
		/// <param name="format">The format used to obscure.</param>
		/// <returns></returns>
		public static String Obscured(this String value, ObscuredTextFormat format)
		{
			var sb = new StringBuilder();
			switch (format)
			{
				case ObscuredTextFormat.FirstAndLastLetterOnly:
					for (var i = 0; i < value.Length; i++)
						sb.Append(((i == 0 || i == (value.Length - 1)) ? value[i] : '*'));
					break;
				case ObscuredTextFormat.FirstLetterOnly:
					for (var i = 0; i < value.Length; i++)
						sb.Append(((i == 0) ? value[i] : '*'));
					break;
				case ObscuredTextFormat.LastFourCharactersOnly:
					for (var i = 0; i < value.Length; i++)
						sb.Append(((i >= value.Length - 4) ? value[i] : '*'));
					break;
				case ObscuredTextFormat.CompletelyObscured:
					for (var i = 0; i < value.Length; i++)
						sb.Append("*");
					break;
			}
			return sb.ToString();
		}

		/// <summary>
		///  Replaces the format item in a specified System.String with the text equivalent
		///  of the value of a specified System.Object instance.
		/// </summary>
		/// <param name="value">A composite format string</param>
		/// <param name="args">An System.Object array containing zero or more objects to format.</param>
		/// <returns>A copy of format in which the format items have been replaced by the System.String
		/// equivalent of the corresponding instances of System.Object in args.</returns>
		public static String Format(this String value, params Object[] args)
		{
			return String.Format(value, args);
		}

		/// <summary>
		/// Converts string to enum object
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <returns>Returns enum object</returns>
		public static T ToEnum<T>(this String value) where T : struct
		{
			return (T)System.Enum.Parse(typeof(T), value, true);
		}
		
		/// <summary>
		/// Checks to see if this string matches the specified regular expression pattern.
		/// </summary>
		/// <param name="text">The string this extension method is extending.</param>
		/// <param name="regexPattern">The regular expression pattern to test against this string.</param>
		/// <returns><c>true</c> is this string matches the provided regular expression pattern; otherwise <c>false</c></returns>
		public static Boolean MatchesPattern(this String text, String regexPattern)
		{
			;
			return new Regex(regexPattern).IsMatch(text);
		}

		/// <summary>
		/// Determines whether this string is a valid credit card number.
		/// </summary>
		/// <remarks>Uses a fast Luhn algorithm to detect if the number is valid.</remarks>
		/// <param name="cardNumber">The string this extension method.</param>
		/// <returns><c>true</c> if this string is a valid credit card number; otherwise <c>false</c></returns>
		/// <see href="http://rosettacode.org/wiki/Luhn_test_of_credit_card_numbers#C.23"/>
		/// <see href="http://orb-of-knowledge.blogspot.com/2009/08/extremely-fast-luhn-function-for-c.html"/>
		public static Boolean IsValidCreditCardNumber(this String cardNumber)
		{
			var deltas = new Int32[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
			var checksum = 0;
			var chars = cardNumber.ToCharArray();
			for (var i = chars.Length - 1; i > -1; i--)
			{
				var j = ((Int32)chars[i]) - 48;
				checksum += j;
				if (((i - chars.Length) % 2) == 0)
					checksum += deltas[j];
			}

			return ((checksum % 10) == 0);
		}

		/// <summary>
		/// Converts this string to a byte array
		/// </summary>
		/// <param name="text">The <see cref="String"/> this method is extending</param>
		/// <returns>Returns a byte array representation of this string</returns>
		public static Byte[] ToByteArray(this String text)
		{
			return Encoding.ASCII.GetBytes(text);
		}

		/// <summary>
		/// Checks this string and attempts to interperate a boolean value from it.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static Boolean ToBoolean(this String text)
		{
			var val = text.ToLower();
			return (val == "y" || val == "yes" || val == "1" || val == "true" || val == "t");
		}

		/// <summary>
		/// Determines whether this instance is numeric.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static Boolean IsNumeric(this String text)
		{
			var number = 0;
			return Int32.TryParse(text, out number);
		}

		/// <summary>
		/// Converts this text to title casing.
		/// </summary>
		/// <param name="text">The string this method is extending</param>
		/// <returns></returns>
		public static String ToTitleCase(this String text)
		{
			var iterateString = text.ToLower();
			var outputString = new StringBuilder(iterateString[0].ToString().ToUpper());

			for (var i = 1; i < iterateString.Length; i++)
				outputString.Append((Char.IsWhiteSpace(iterateString, i - 1)) ? iterateString[i].ToString().ToUpper() : iterateString[i].ToString().ToLower());

			return outputString.ToString();
		}
		
		/// <summary>
		/// Performs a String.Format() on this string with the provided arguements
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="args">Arguments to pass to the format string.</param>
		/// <returns>Returns a formatted string</returns>
		public static String DoFormat(this String text, params Object[] args)
		{
			return String.Format(text, args);
		}

		/// <summary>
		/// Determines whether this string is null or white space
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <returns><c>true</c> if this string is null or white space; otherwise <c>false</c></returns>
		public static Boolean IsNullOrWhiteSpace(this String text)
		{
			return text == null || text.Trim().Length == 0;
		}

		/// <summary>
		/// Strips characters from this string except for numbers.
		/// </summary>
		/// <param name="text">The text this method extends.</param>
		/// <returns></returns>
		public static String StripAllButNumbers(this String text)
		{
			return Regex.Replace(text, @"[^0-9]", "");
		}

		/// <summary>
		/// Reverses this string.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <returns></returns>
		public static String Reverse(this String text)
		{
			var charArray = text.ToCharArray();
			Array.Reverse(charArray);
			return new String(charArray);
		}
		
		/// <summary>
		/// This tries to detect a json string, this is not a fail safe way but it is quicker than doing
		/// a try/catch when deserializing when it is not json.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <returns><c>true</c> if this string is JSON; otherwise <c>false</c></returns>
		public static Boolean DetectIsJson(this String text)
		{
			text = text.Trim();
			return (text.StartsWith("{") && text.EndsWith("}"))
				   || (text.StartsWith("[") && text.EndsWith("]"));
		}

		/// <summary>
		/// Returns a stream from a string
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <returns></returns>
		public static Stream GenerateStreamFromString(this String text)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(text);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		/// <summary>
		/// Trims the specified text away from the end of this string.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="forRemoving">The text to remove form the end of this string.</param>
		/// <returns>Returns this string without the specified text at the end</returns>
		public static String TrimEnd(this String text, String forRemoving)
		{
			if (String.IsNullOrEmpty(text)) 
				return text;
			
			if (String.IsNullOrEmpty(forRemoving)) 
				return text;

			while (text.EndsWith(forRemoving, StringComparison.CurrentCultureIgnoreCase))
			{
				text = text.Remove(text.LastIndexOf(forRemoving, StringComparison.CurrentCultureIgnoreCase));
			}
			return text;
		}

		/// <summary>
		/// Trims the specified text away from the start of this string.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="forRemoving">The text to remove form the start of this string.</param>
		/// <returns>Returns this string without the specified text at the start</returns>
		public static String TrimStart(this String text, String forRemoving)
		{
			if (String.IsNullOrEmpty(text)) 
				return text;

			if (String.IsNullOrEmpty(forRemoving)) 
				return text;

			while (text.StartsWith(forRemoving, StringComparison.CurrentCultureIgnoreCase))
			{
				text = text.Substring(forRemoving.Length);
			}
			return text;
		}

		/// <summary>
		/// Ensures that this string will start with the specified text.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="toStartWith">The text to ensure exists at the start of this string.</param>
		/// <returns>Returns this string with the start text applied if it wasn't already</returns>
		public static String EnsureStartsWith(this String text, String toStartWith)
		{
			if (text.StartsWith(toStartWith)) return text;
			return toStartWith + text.TrimStart(toStartWith);
		}

		/// <summary>
		/// Ensures that this string will start with the specified text.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="toStartWith">The text to ensure exists at the start of this string.</param>
		/// <returns>Returns this string with the start text applied if it wasn't already</returns>
		public static String EnsureStartsWith(this String text, Char toStartWith)
		{
			return text.StartsWith(toStartWith.ToString()) ? text : toStartWith + text;
		}

		/// <summary>
		/// Ensures that this string will end with the specified text.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="toEndWith">The text to ensure exists at the end of this string.</param>
		/// <returns>Returns this string with the end text applied if it wasn't already</returns>
		public static String EnsureEndsWith(this String text, Char toEndWith)
		{
			return text.EndsWith(toEndWith.ToString()) ? text : text + toEndWith;
		}

		/// <summary>
		/// Ensures that this string will end with the specified text.
		/// </summary>
		/// <param name="text">The string this method is extending.</param>
		/// <param name="toEndWith">The text to ensure exists at the end of this string.</param>
		/// <returns>Returns this string with the end text applied if it wasn't already</returns>
		public static String EnsureEndsWith(this String text, String toEndWith)
		{
			return text.EndsWith(toEndWith) ? text : text + toEndWith;
		}


		#endregion EXTENSION METHODS

	}


	/// <summary>
	/// The type of ways a string can be obscured.
	/// </summary>
	public enum ObscuredTextFormat
	{
		/// <summary>
		/// Format for obscuring everything but the first letter of the string.
		/// </summary>
		FirstLetterOnly,
		/// <summary>
		/// Format for obscuring everything but the first and last letters of the string.
		/// </summary>
		FirstAndLastLetterOnly,
		/// <summary>
		/// Format for obscuring everything but the last four characters of the string.
		/// </summary>
		LastFourCharactersOnly,
		/// <summary>
		/// Format for completely obscuring all characters of the string.
		/// </summary>
		CompletelyObscured
	}



}
