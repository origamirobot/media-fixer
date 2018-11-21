using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFixer.Core.Terminal
{

	/// <summary>
	/// 
	/// </summary>
	public class ConsoleMenuList
	{
		
		#region PRIVATE PROPERTIES


		private ConsoleListItem[][] _itemMatrix;
		private Int32 _top = 0;
		private Int32 _bottom = 0;


		#endregion PRIVATE PROPERTIES
		
		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets the selected item in this list
		/// </summary>
		public ConsoleListItem SelectedItem
		{
			get { return this.Items[this.SelectedIndex]; }
		}

		/// <summary>
		/// Gets or sets the ammount of columns to display for menu items
		/// </summary>
		public Int32 GridWidth { get; set; } = 3;

		/// <summary>
		/// The style of the border to render
		/// </summary>
		public ConsoleBorderStyle BorderStyle { get; set; }

		/// <summary>
		/// The width to make each idividual item
		/// </summary>
		public Int32 ItemWidth { get; set; } = 20;

		/// <summary>
		/// A collection of ConsoleListItems in this control
		/// </summary>
		public List<ConsoleListItem> Items { get; } = new List<ConsoleListItem>();

		/// <summary>
		/// The index of the selected item
		/// </summary>
		public Int32 SelectedIndex { get; set; }

		/// <summary>
		/// Gets the Value of the selected item
		/// </summary>
		public Object SelectedValue
		{
			get { return this.Items[this.SelectedIndex].Value; }
		}

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
		/// Gets or sets the forecolor of this control
		/// </summary>
		public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;


		#endregion PUBLIC ACCESSORS
		
		#region CONSTRUCTORS


		/// <summary>
		/// Creates an instance of the ConsoleMenuList class
		/// </summary>
		public ConsoleMenuList()
		{

		}


		#endregion CONSTRUCTORS
		
		#region PRIVATE METHODS


		/// <summary>
		/// Takes the items in the item list and creates a matrix using the GridWidth property
		/// </summary>
		private void BuildItemMatrix()
		{
			var items = new List<ConsoleListItem[]>();
			var flag = true;
			var count = 0;
			while (flag)
			{
				var row = new List<ConsoleListItem>();
				for (var i = 0; i < this.GridWidth; i++)
				{
					if (count == this.SelectedIndex)
						this.Items[count].Selected = true;
					else
						this.Items[count].Selected = false;
					row.Add(this.Items[count]);
					if ((count + 1) == this.Items.Count)
					{
						flag = false;
						break;
					}
					count++;
				}
				items.Add(row.ToArray());
			}
			this._itemMatrix = items.ToArray();
		}

		/// <summary>
		/// Builds the table to store all the items in
		/// </summary>
		private void BuildLayout()
		{
			this._top = System.Console.CursorTop;
			System.Console.BackgroundColor = this.BackColor;
			System.Console.ForegroundColor = this.BorderColor;

			var list = new BorderCharList(this.BorderStyle);
			var width = (this.GridWidth * this.ItemWidth) + (this.GridWidth - 1);

			if (this._itemMatrix.Length == 0)
				return;


			// LOOP THE ROWS IN THE ITEM MATRIX
			for (var i = 0; i < this._itemMatrix.Length; i++)
			{
				// CHECK IF THIS IS THE FIRST ROW
				if (i == 0)
				{
					System.Console.Write(list.UpperLeft);
					// LOOP EACH ITEM IN THIS ROW
					for (var k = 0; k < this.GridWidth; k++)
					{
						for (var j = 0; j < ItemWidth; j++)
							System.Console.Write(list.OuterHorizontal);
						if ((k + 1) != this.GridWidth)
							System.Console.Write(list.OuterBreakDown);
					}
					System.Console.Write(list.UpperRight + "\n");
				}

				// LOOP THE ITEMS IN THIS ROW
				for (var j = 0; j < this.GridWidth; j++)
				{
					if (j == 0)
						System.Console.Write(list.OuterVertical);

					for (var l = 0; l < this.ItemWidth; l++)
						System.Console.Write(" ");

					if ((j + 1) == this.GridWidth)
						System.Console.Write(list.OuterVertical + "\n");
					else
						System.Console.Write(list.InnerVertical);
				}


				// CHECK IF THIS IS THE LAST ROW
				if ((i + 1) == this._itemMatrix.Length)
				{
					System.Console.Write(list.LowerLeft);
					// LOOP EACH ITEM IN THIS ROW
					for (var k = 0; k < this.GridWidth; k++)
					{
						for (var j = 0; j < ItemWidth; j++)
							System.Console.Write(list.OuterHorizontal);
						if ((k + 1) != this.GridWidth)
							System.Console.Write(list.OuterBreakUp);
					}
					System.Console.Write(list.LowerRight + "\n");
				}
				else
				{
					System.Console.Write(list.OuterBreakRight);
					// LOOP EACH ITEM IN THIS ROW
					for (var k = 0; k < this.GridWidth; k++)
					{
						for (var j = 0; j < ItemWidth; j++)
							System.Console.Write(list.InnerHorizontal);
						if ((k + 1) != this.GridWidth)
							System.Console.Write(list.CrossSection);
					}
					System.Console.Write(list.OuterBreakLeft + "\n");
				}
			}
			this._bottom = System.Console.CursorTop;

		}

		/// <summary>
		/// Places the items in the boxes
		/// </summary>
		private void PlaceItems()
		{
			System.Console.CursorTop = this._top + 1;
			System.Console.CursorLeft = 2;
			// LOOP ITEM ROWS
			foreach (var item in this._itemMatrix)
			{
				System.Console.CursorLeft = 1;
				// LOOP ITEM COLUMNS
				foreach (var inner in item)
				{
					var left = System.Console.CursorLeft;

					if (inner.Selected)
					{
						System.Console.BackgroundColor = this.SelectedBackColor;
						System.Console.ForegroundColor = this.SelectedForeColor;
					}
					else
					{
						System.Console.BackgroundColor = this.ItemBackColor;
						System.Console.ForegroundColor = this.ItemForeColor;
					}

					System.Console.Write(inner.Text.Length > this.ItemWidth 
						? inner.Text.Substring(0, this.ItemWidth) 
						: inner.Text.PadRight(this.ItemWidth));

					System.Console.CursorLeft = left + ItemWidth + 1;
				}
				System.Console.CursorTop += 2;
				System.Console.BackgroundColor = this.ItemBackColor;
				System.Console.ForegroundColor = this.ItemForeColor;
			}


		}

		/// <summary>
		/// Cleans up the console
		/// </summary>
		private void CleanUp()
		{
			System.Console.CursorTop = this._bottom;
			System.Console.BackgroundColor = System.ConsoleColor.Black;
			System.Console.ForegroundColor = System.ConsoleColor.White;
		}


		#endregion PRIVATE METHODS
		
		#region PUBLIC METHODS


		/// <summary>
		/// Executes this control in the console
		/// </summary>
		public void Execute()
		{
			System.Console.CursorVisible = false;
			BuildItemMatrix();
			BuildLayout();
			PlaceItems();
			// TRAP USER INPUT
			while (true)
			{
				var info = System.Console.ReadKey();
				switch (info.Key)
				{
					case System.ConsoleKey.RightArrow:
						// RIGHT ARROW IS SelectedIndex + 1
						SelectedIndex += 1;
						break;
					case System.ConsoleKey.LeftArrow:
						// LEFT ARROW IS SelectedIndex - 1
						SelectedIndex -= 1;
						break;
					case System.ConsoleKey.UpArrow:
						SelectedIndex -= GridWidth;
						break;
					case System.ConsoleKey.DownArrow:
						SelectedIndex += GridWidth;
						break;
					case System.ConsoleKey.Enter:
						CleanUp();
						return;

				}
				// CHECK TO MAKE SURE WE HAVENT NAVIGATED OUTSIDE THE BOUNDS OF THE ITEM MATRIX
				if ((SelectedIndex + 1) > this.Items.Count)
					SelectedIndex = (this.Items.Count - 1);
				if (SelectedIndex < 0)
					SelectedIndex = 0;

				BuildItemMatrix();
				PlaceItems();
			}
		}


		#endregion PUBLIC METHODS

	}
}
