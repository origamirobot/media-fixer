using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFixer.Core.Terminal
{


	/// <summary>
	/// Creates a banner of text in the console using ASCII text
	/// </summary>
	public class ConsoleBanner
	{

		#region PRIVATE PROPERTIES

		private Boolean _fontBold = true;


		#endregion PRIVATE PROPERTIES

		#region PUBLIC ACCESSORS


		/// <summary>
		/// Gets or sets the foreground color of the console when this control is being executed
		/// </summary>
		/// <value>ConsoleColor.White</value>
		public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;

		/// <summary>
		/// Gets or sets the background color of the console when this control is being executed
		/// </summary>
		/// <value>ConsoleColor.Black</value>
		public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;

		/// <summary>
		/// Gets or sets the text to be rendered in the console as a banner
		/// </summary>
		/// <value>string.Empty</value>
		public String Text { get; set; } = String.Empty;

		/// <summary>
		/// Gets or sets the pallet of characters to use when rendering this text to the console as a banner
		/// </summary>
		public Char[] Pallet { get; set; } = new Char[] { '█', ' ' };

		/// <summary>
		/// Gets or sets the font used when rendering the text as ASCII in the console
		/// </summary>
		/// <value>Arial, 9, FontStyle.Regular, GraphicsUnit.Pixel</value>
		public Font Font { get; set; } = new Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Pixel);

		/// <summary>
		/// Gets or sets the width of this banner in characters
		/// </summary>
		public Int32 Width { get; set; } = 60;

		/// <summary>
		/// Gets or sets the height of this banner in characters
		/// </summary>
		public Int32 Height { get; set; } = 12;


		#endregion PUBLIC ACCESSORS

		#region CONSTRUCTORS


		/// <summary>
		/// Creates an instance of he console banner control
		/// </summary>
		public ConsoleBanner()
		{

		}

		/// <summary>
		/// Creates an instance of he console banner control
		/// </summary>
		public ConsoleBanner(String text)
		{
			this.Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleBanner" /> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="fontFamily">The font family.</param>
		/// <param name="fontSize">Size of the font.</param>
		/// <param name="fontStyle">The font style.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public ConsoleBanner(String text, String fontFamily, Single fontSize, FontStyle fontStyle, Int32 width, Int32 height)
		{
			this.Text = text;
			this.Font = new Font(fontFamily, fontSize, fontStyle);
			this.Width = width;
			this.Height = height;
		}


		#endregion CONSTRUCTORS

		#region PUBLIC METHODS


		/// <summary>
		/// Executes this control in the console window
		/// </summary>
		public void Execute()
		{
			// CREATE A BLANK IMAGE TO THE SIZE OF THIS CONTROL
			var map = new Bitmap(this.Width, this.Height);
			var g = Graphics.FromImage(map);
			Brush fillBrush = new SolidBrush(Color.White);
			g.FillRectangle(fillBrush, 0, 0, this.Width, this.Height);
			Brush brush = new SolidBrush(Color.Black);
			// DRAW THE STRING ONTO THE BLANK CANVAS
			g.DrawString(this.Text, this.Font, brush, 0,0);
			// DISPOSE OF THE GRAPHICS OBJECT
			g.Dispose();

			var art = String.Empty;
			var width = this.Width;
			var height = this.Height;

			var countH = 0;
			// LOOP THE IMAGE PIXEL MATRIX VERTICALLY
			for (countH = 0; countH < height; countH++)
			{
				var line = "";
				// LOOP THE IMAGE PIXEL MATRIX HORIZONTALLY
				var countW = 0;
				for (countW = 0; countW < width; countW++)
				{
					// GET THE BRIGHTNESS OF THE CURRENT PIXEL
					var pixelBrightness = Convert.ToInt32(map.GetPixel(countW, countH).GetBrightness() * 100);
					// STEP IS THE PERCENT EACH CHARACTER TAKES IN THE PALLET
					var step = Convert.ToInt32(100 / Pallet.Length);
					var count = 0;
					var selectedChar = ' ';
					// LOOP OVER ALL THE CHARACTERS IN THE PALLET
					foreach (var palletValue in Pallet)
					{
						var currentValue = count * step;
						if (currentValue > pixelBrightness)
							break;
						else
							selectedChar = palletValue;
						count++;
					}
					line += selectedChar.ToString();
				}
				//File.AppendAllText(@"C:\Output.txt", line + "\n");
				art += line + "\n";
			}

			// WRITE THE FINISHED ASCII ART TO THE CONSOLE
			System.Console.ForegroundColor = this.ForeColor;
			System.Console.BackgroundColor = this.BackColor;
			System.Console.Write(art);
			// RESET THE CONSOLE COLOR BACK TO THE DEFAULTS
			System.Console.ForegroundColor = ConsoleColor.White;
			System.Console.BackgroundColor = ConsoleColor.Black;
			// DISPOSE OF THE TEMPORARY IMAGE
			map.Dispose();
		}


		#endregion PUBLIC METHODS

	}

}
