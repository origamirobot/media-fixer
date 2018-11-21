using System;
using System.Collections.Generic;

namespace MediaFixer.Core.Terminal
{

	/// <summary>
	/// Represents a select list in the console
	/// </summary>
	public class ConsoleSelectList
	{
		
		#region PRIVATE PROPERTIES


		private Int32 _firstLine = 0;
		private Int32 _lastLine = 0;
		private ConsoleColor _originalFore = Console.ForegroundColor;
		private ConsoleColor _originalBack = Console.BackgroundColor;


		#endregion PRIVATE PROPERTIES
		
		#region PUBLIC ACCESSORS

		/// <summary>
		/// Gets the selected item from the list of items in this control
		/// </summary>
		public ConsoleListItem SelectedItem => Items[SelectedIndex];

		/// <summary>
		/// Gets or sets the background color of the selected item
		/// </summary>
		public ConsoleColor SelectedBackColor { get; set; } = ConsoleColor.DarkBlue;

		/// <summary>
		/// Gets or sets the foreground color of the selected item
		/// </summary>
		public ConsoleColor SelectedForeColor { get; set; } = ConsoleColor.White;

		/// <summary>
		/// Gets or sets the background color of the each non selected item
		/// </summary>
		public ConsoleColor ItemBackColor { get; set; } = ConsoleColor.Black;

		/// <summary>
		/// Gets or sets the foreground color of the each non selected item
		/// </summary>
		public ConsoleColor ItemForeColor { get; set; } = ConsoleColor.White;

		/// <summary>
		/// Gets or sets the background color of the title
		/// </summary>
		public ConsoleColor TitleBackColor { get; set; } = ConsoleColor.DarkBlue;

		/// <summary>
		/// Gets or sets the foreground color of the title
		/// </summary>
		public ConsoleColor TitleForeColor { get; set; } = ConsoleColor.White;

		/// <summary>
		/// Gets or sets the background color of the control
		/// </summary>
		public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;

		/// <summary>
		/// Gets or sets the color of the border
		/// </summary>
		public ConsoleColor BorderColor { get; set; } = ConsoleColor.White;

		/// <summary>
		/// A list of select list items in this control
		/// </summary>
		public List<ConsoleListItem> Items { get; set; } = new List<ConsoleListItem>();

		/// <summary>
		/// The index of the selected item
		/// </summary>
		public Int32 SelectedIndex { get; set; } = 0;

		/// <summary>
		/// The width in characters to display this object
		/// </summary>
		public Int32 Width { get; set; } = 60;

		/// <summary>
		/// Gets or sets the style of the border 
		/// </summary>
		public ConsoleBorderStyle BorderStyle { get; set; } = ConsoleBorderStyle.SingleDouble;

		/// <summary>
		/// Gets the selected items value
		/// </summary>
		public Object SelectedValue => Items[SelectedIndex].Value;

		/// <summary>
		/// Gets or sets a value indicating if the title should be shown
		/// </summary>
		public Boolean ShowTitle { get; set; } = false;

		/// <summary>
		/// Gets or sets the title to show at the top of this control
		/// </summary>
		public String Title { get; set; } = String.Empty;

		/// <summary>
		/// Gets or sets the forecolor of this control
		/// </summary>
		public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;


		#endregion PUBLIC ACCESSORS
		
		#region CONSTRUCTORS


		/// <summary>
		/// Creates an instance of the select list
		/// </summary>
		public ConsoleSelectList()
		{

		}


		#endregion CONSTRUCTORS
		
		#region PRIVATE METHODS


		/// <summary>
		/// Creates the table that holds the items
		/// </summary>
		private void CreateTable()
		{
			var list = new BorderCharList(BorderStyle);
			Console.ForegroundColor = BorderColor;
			Console.BackgroundColor = BackColor;

			Console.Write(list.UpperLeft);
			for (var j = 0; j < Width; j++)
				Console.Write(list.OuterHorizontal);
			Console.Write(list.UpperRight + "\n");

			if (ShowTitle)
			{
				Console.Write(list.OuterVertical);
				Console.BackgroundColor = TitleBackColor;
				Console.ForegroundColor = TitleForeColor;
				for (var i = 0; i < Width; i++)
					Console.Write(" ");
				Console.CursorLeft = FindCenter(Title);
				Console.Write(Title);
				Console.CursorLeft = (Width + 1);

				Console.ForegroundColor = BorderColor;
				Console.BackgroundColor = BackColor;
				Console.Write(list.OuterVertical + "\n");

				Console.Write(list.OuterBreakRight);
				for (var j = 0; j < Width; j++)
					Console.Write(list.InnerHorizontal);
				Console.Write(list.OuterBreakLeft + "\n");
			}

			for (var i = 0; i < Items.Count; i++)
			{
				Console.Write(list.OuterVertical.ToString());
				for (var j = 0; j < Width; j++)
					Console.Write(" ");
				Console.Write(list.OuterVertical + "\n");
				if ((i + 1) != Items.Count)
				{
					Console.Write(list.OuterBreakRight);
					for (var j = 0; j < Width; j++)
						Console.Write(list.InnerHorizontal);
					Console.Write(list.OuterBreakLeft + "\n");
				}

			}

			Console.Write(list.LowerLeft);
			for (var j = 0; j < Width; j++)
				Console.Write(list.OuterHorizontal);
			Console.Write(list.LowerRight + "\n");
			_lastLine = Console.CursorTop;

		}

		/// <summary>
		/// Places the items into the created table
		/// </summary>
		private void PlaceItems()
		{
			Console.CursorTop = _firstLine;
			// LOOP EACH ITEM IN THE ITEMS LIST
			for (var i = 0; i < Items.Count; i++)
			{
				Console.CursorTop = (i * 2) + _firstLine + 1;
				Console.CursorLeft = 1;
				if (ShowTitle)
					Console.CursorTop += 2;
				// CHECK IF THIS IS THE SELECTED ITEM
				if (SelectedIndex == i)
				{
					Console.BackgroundColor = SelectedBackColor;
					Console.ForegroundColor = SelectedForeColor;
					for (var j = 0; j < Width; j++)
						Console.Write(" ");
					Console.CursorLeft = 1;
					Console.Write(" " + Items[i].Text);
				}
				else
				{
					Console.BackgroundColor = ItemBackColor;
					Console.ForegroundColor = ItemForeColor;
					Console.Write(" " + Items[i].Text.PadRight((Width - 1)));
					Console.CursorLeft = 1;
				}
			}
		}

		/// <summary>
		/// Cleans up the console, and returns it back to its original settings
		/// before this control was executed
		/// </summary>
		private void CleanUp()
		{
			// RESET THE CURSOR
			Console.CursorVisible = true;
			Console.CursorTop = _lastLine + 2;

			// RESET COLORS BACK TO ORIGINAL COLORS
			Console.BackgroundColor = _originalBack;
			Console.ForegroundColor = _originalFore;
		}

		/// <summary>
		/// Finds the position to put the text in the title
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private Int32 FindCenter(String text)
		{
			var width = Width;
			var size = text.Length;
			return Convert.ToInt32(Math.Ceiling(((Double)width / 2)) - Math.Ceiling(((Double)size / 2)));
		}


		#endregion PRIVATE METHODS
		
		#region PUBLIC METHODS


		/// <summary>
		/// Renders this control to the console
		/// </summary>
		public void Execute()
		{
			_firstLine = Console.CursorTop;
			Console.CursorVisible = false;
			// CREATE THE TABLE
			CreateTable();
			// RENDER THE ITEMS
			PlaceItems();
			// TRAP THE KEY STROKES
			while (true)
			{
				// CAPTURE THE KEY THAT WAS PRESSED AND FIND OUT WHAT IT WAS
				var key = Console.ReadKey().Key;
				switch (key)
				{
					case ConsoleKey.UpArrow: SelectedIndex--; break;
					case ConsoleKey.DownArrow: SelectedIndex++; break;
					case ConsoleKey.RightArrow: SelectedIndex++; break;
					case ConsoleKey.LeftArrow: SelectedIndex--; break;
					case ConsoleKey.Enter:
						CleanUp();
						return;
				}
				// KEEP THE INDEX WITHIN THE RANGE NEEDED
				if ((SelectedIndex + 1) > Items.Count)
					SelectedIndex = 0;
				if (SelectedIndex < 0)
					SelectedIndex = Items.Count - 1;
				PlaceItems();
			}

		}


		#endregion PUBLIC METHODS


	}


	/// <summary>
	/// Represents a single item in a Console List
	/// </summary>
	public class ConsoleListItem : IComparable<ConsoleListItem>
	{

		#region PUBLIC ACCESSORS


		/// <summary>
		/// The text that is displayed when this item is rendered
		/// </summary>
		public String Text { get; set; }

		/// <summary>
		/// The value of this item
		/// </summary>
		public Object Value { get; set; }

		/// <summary>
		/// The width of this list item
		/// </summary>
		public Int32 Width { get; set; }

		/// <summary>
		/// Gets or sets a value indicating if this item is selected
		/// </summary>
		public Boolean Selected { get; set; } = false;


		#endregion PUBLIC ACCESSORS
		
		#region CONSTRUCTORS


		/// <summary>
		/// Creates an instance of the console select item class
		/// </summary>
		public ConsoleListItem()
		{

		}

		/// <summary>
		/// Creates an instance of the console select item class
		/// </summary>
		/// <param name="text">The value to set this instances Text property</param>
		public ConsoleListItem(String text)
		{
			Text = text;
		}

		/// <summary>
		/// Creates an instance of the console select item class
		/// </summary>
		/// <param name="text">The value to set this instances Text property</param>
		/// <param name="value">The value to set this instances Value property</param>
		public ConsoleListItem(String text, Object value)
		{
			Text = text;
			Value = value;
		}


		#endregion CONSTRUCTORS
		
		#region OVERRIDDEN METHODS


		/// <summary>
		/// Returns a System.String that represents this ConsoleListItem
		/// </summary>
		/// <returns>A System.String that represents this ConsoleListItem</returns>
		public override String ToString()
		{
			return Text;
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
		/// </returns>
		public Int32 CompareTo(ConsoleListItem other)
		{
			return String.Compare(Text, other.Text, StringComparison.Ordinal);
		}


		#endregion OVERRIDDEN METHODS

	}

	/// <summary>
	/// Contains a collection of characters to use when creating borders
	/// </summary>
	public struct BorderCharList
	{
		
		#region PRIVATE PROPERTIES

		#endregion PRIVATE PROPERTIES

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Upper left corner of the outside border
		/// </summary>
		public Char UpperLeft { get; set; }

		/// <summary>
		/// Upper right corner of the outside border
		/// </summary>
		public Char UpperRight { get; set; }

		/// <summary>
		/// Lower left corner of the outside border
		/// </summary>
		public Char LowerLeft { get; set; }

		/// <summary>
		/// Lower right corner of the outside border
		/// </summary>
		public Char LowerRight { get; set; }

		/// <summary>
		/// Vertical outside border
		/// </summary>
		public Char OuterVertical { get; set; }

		/// <summary>
		/// Horizontal outside border
		/// </summary>
		public Char OuterHorizontal { get; set; }

		/// <summary>
		/// Inner vertical border
		/// </summary>
		public Char InnerVertical { get; set; }

		/// <summary>
		/// Inner horizontal border
		/// </summary>
		public Char InnerHorizontal { get; set; }

		/// <summary>
		/// Inner cross section that breaks upward
		/// </summary>
		public Char CrossBreakUp { get; set; }

		/// <summary>
		/// Inner cross section that breaks to the right
		/// </summary>
		public Char CrossBreakRight { get; set; }

		/// <summary>
		/// Inner cross section that breaks to the left
		/// </summary>
		public Char CrossBreakLeft { get; set; }

		/// <summary>
		/// Inner cross section that breaks downward
		/// </summary>
		public Char CrossBreakDown { get; set; }

		/// <summary>
		/// Inner cross section breaking in all four ways
		/// </summary>
		public Char CrossSection { get; set; }

		/// <summary>
		/// Outer border that breaks right
		/// </summary>
		public Char OuterBreakRight { get; set; }

		/// <summary>
		/// Outer border that breaks left
		/// </summary>
		public Char OuterBreakLeft { get; set; }

		/// <summary>
		/// Outer border that breaks downward
		/// </summary>
		public Char OuterBreakDown { get; set; }

		/// <summary>
		/// Outer border that breaks upward
		/// </summary>
		public Char OuterBreakUp { get; set; }


		#endregion PUBLIC ACCESSORS

		#region CONSTRUCTORS


		/// <summary>
		/// Creates an instance of the BorderCharList
		/// </summary>
		/// <param name="style">The style of the border to create a charlist for</param>
		public BorderCharList(ConsoleBorderStyle style)
		{
			switch (style)
			{
				case ConsoleBorderStyle.BlockLine:
					CrossBreakDown = '█';
					CrossBreakLeft = '█';
					CrossBreakRight = '█';
					CrossBreakUp = '█';
					CrossSection = '█';
					InnerHorizontal = '█';
					InnerVertical = '█';
					LowerLeft = '█';
					LowerRight = '█';
					OuterHorizontal = '█';
					OuterVertical = '█';
					UpperLeft = '█';
					UpperRight = '█';
					OuterBreakRight = '█';
					OuterBreakDown = '█';
					OuterBreakLeft = '█';
					OuterBreakUp = '█';
					break;
				case ConsoleBorderStyle.DoubleLine:
					CrossBreakDown = '╦';
					CrossBreakLeft = '╣';
					CrossBreakRight = '╠';
					CrossBreakUp = '╩';
					CrossSection = '╬';
					InnerHorizontal = '═';
					InnerVertical = '║';
					LowerLeft = '╚';
					LowerRight = '╝';
					OuterHorizontal = '═';
					OuterVertical = '║';
					UpperLeft = '╔';
					UpperRight = '╗';
					OuterBreakRight = '╠';
					OuterBreakDown = '╦';
					OuterBreakLeft = '╣';
					OuterBreakUp = '╩';
					break;
				case ConsoleBorderStyle.SingleDouble:
					CrossBreakDown = '┬';
					CrossBreakLeft = '┤';
					CrossBreakRight = '├';
					CrossBreakUp = '┴';
					CrossSection = '┼';
					InnerHorizontal = '─';
					InnerVertical = '│';
					LowerLeft = '╚';
					LowerRight = '╝';
					OuterHorizontal = '═';
					OuterVertical = '║';
					UpperLeft = '╔';
					UpperRight = '╗';
					OuterBreakRight = '╟';
					OuterBreakDown = '╤';
					OuterBreakLeft = '╢';
					OuterBreakUp = '╧';
					break;
				case ConsoleBorderStyle.SingleLine:
					CrossBreakDown = '┬';
					CrossBreakLeft = '┤';
					CrossBreakRight = '├';
					CrossBreakUp = '┴';
					CrossSection = '┼';
					InnerHorizontal = '─';
					InnerVertical = '│';
					LowerLeft = '└';
					LowerRight = '┘';
					OuterHorizontal = '─';
					OuterVertical = '│';
					UpperLeft = '┌';
					UpperRight = '┐';
					OuterBreakRight = '├';
					OuterBreakDown = '┬';
					OuterBreakLeft = '┤';
					OuterBreakUp = '┴';
					break;
				case ConsoleBorderStyle.None:
					CrossBreakDown = ' ';
					CrossBreakLeft = ' ';
					CrossBreakRight = ' ';
					CrossBreakUp = ' ';
					CrossSection = ' ';
					InnerHorizontal = ' ';
					InnerVertical = ' ';
					LowerLeft = ' ';
					LowerRight = ' ';
					OuterHorizontal = ' ';
					OuterVertical = ' ';
					UpperLeft = ' ';
					UpperRight = ' ';
					OuterBreakRight = ' ';
					OuterBreakDown = ' ';
					OuterBreakLeft = ' ';
					OuterBreakUp = ' ';
					break;
				default:
					CrossBreakDown = '█';
					CrossBreakLeft = '█';
					CrossBreakRight = '█';
					CrossBreakUp = '█';
					CrossSection = '█';
					InnerHorizontal = '█';
					InnerVertical = '█';
					LowerLeft = '█';
					LowerRight = '█';
					OuterHorizontal = '█';
					OuterVertical = '█';
					UpperLeft = '█';
					UpperRight = '█';
					OuterBreakRight = '█';
					OuterBreakDown = '█';
					OuterBreakLeft = '█';
					OuterBreakUp = '█';
					break;

			}
		}


		#endregion CONSTRUCTORS

	}

	/// <summary>
	/// Different types of border that can be drawn.
	/// </summary>
	public enum ConsoleBorderStyle
	{
		None,
		SingleLine,
		SingleDouble,
		DoubleLine,
		BlockLine
	}

}


