using System;
using System.Collections.Generic;
using System.IO;
using MediaFixer.Core.Configuration;
using MediaFixer.Core.Fixers;
using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
using MediaFixer.Core.Terminal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MediaFixer.Core.Tests.Fixers
{

	[TestClass]
	public class MovieFixerTests
	{

		#region PROTECTED PROPERTIES


		protected Mock<IMovieConfiguration> Settings { get; set; }
		protected Mock<ILogger> Logger { get; set; }
		protected Mock<IFileUtility> FileUtility { get; set; }
		protected Mock<IDirectoryUtility> DirectoryUtility { get; set; }
		protected Mock<IPathUtility> PathUtility { get; set; }
		protected Mock<IConsole> Console { get; set; }
		protected MovieFixer MovieFixer { get; set; }


		#endregion PROTECTED PROPERTIES

		#region SETUP


		[TestInitialize]
		public void Setup()
		{
			Settings = new Mock<IMovieConfiguration>();
			Logger = new Mock<ILogger>();
			FileUtility = new Mock<IFileUtility>();
			DirectoryUtility = new Mock<IDirectoryUtility>();
			PathUtility = new Mock<IPathUtility>();
			Console = new Mock<IConsole>();
			Settings.Setup(x => x.MovieRegex).Returns(@"(?<title>.*)(?<year>19\d{2}|20\d{2})");
			Settings.Setup(x => x.MovieYearRegex).Returns(@"(19|20)\d{2}");
			Settings.Setup(x => x.CharactersToReplace).Returns(new List<String>() {"[", "]", "{", "}", "(", ")", "~", "`", "."});
			MovieFixer = new MovieFixer(Settings.Object, Logger.Object, DirectoryUtility.Object, FileUtility.Object, PathUtility.Object, Console.Object);
		}


		#endregion SETUP

		#region TESTS


		[TestMethod]
		public void Parse_method_should_correctly_identify_movie_parts()
		{
			var files = new List<MovieRegexValue>
			{
				new MovieRegexValue(){ Name = "Skate.Kitchen.(2018).1080p.WEB-DL.DD5.1.H264-CMRG", ExpectedTitle = "Skate Kitchen", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Christopher.Robin.2018.BDRip.DD5.1.x264-playSD", ExpectedTitle = "Christopher Robin", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "The.Predator.2018.1080p.HC.HDRip.x264.AC3-EVO", ExpectedTitle = "The Predator", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Comedy.Natasha.Leggero.-.Live.at.Bimbos.2015.1080p.WEB-DL.DD+.2.0.x264-TrollHD", ExpectedTitle = "Comedy Natasha Leggero - Live At Bimbos", ExpectedYear = 2015},
				new MovieRegexValue(){ Name = "Beatbox.2015.1080p.AMZN.WEB-DL.DDP5.1.H.264-MZABI", ExpectedTitle = "Beatbox", ExpectedYear = 2015},
				new MovieRegexValue(){ Name = "8.Mile.2002.720p.BluRay.x264-BestHD", ExpectedTitle = "8 Mile", ExpectedYear = 2002},
				new MovieRegexValue(){ Name = "A.Dangerous.Idea.2016.1080p.AMZN.WEB-DL.DDP2.0.H.264-MZABI", ExpectedTitle = "A Dangerous Idea", ExpectedYear = 2016},
				new MovieRegexValue(){ Name = "A.Midsummer.Nights.Dream.2018.HDRip.XviD", ExpectedTitle = "A Midsummer Nights Dream", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Beyond.Clueless.2014.DVDRip.x264-REDBLADE", ExpectedTitle = "Beyond Clueless", ExpectedYear = 2014},
				new MovieRegexValue(){ Name = "Kodachrome.2018.BDRip.XviD", ExpectedTitle = "Kodachrome", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Olafs.Frozen.Adventure.2017.1080p.BluRay.x264-HANDJOB", ExpectedTitle = "Olafs Frozen Adventure", ExpectedYear = 2017},
				new MovieRegexValue(){ Name = "Pepermint.2018.HDRip.XviD", ExpectedTitle = "Pepermint", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Kansas.1995.DVDRip.x264-HANDJOB", ExpectedTitle = "Kansas", ExpectedYear = 1995},
				new MovieRegexValue(){ Name = "Gas.Food.Lodging.1992.INTERNAL.BDRip.x264-GHOULS", ExpectedTitle = "Gas Food Lodging", ExpectedYear = 1992},
				new MovieRegexValue(){ Name = "Girl.Interrupted.1999.WEB-DL.720P.H264", ExpectedTitle = "Girl Interrupted", ExpectedYear = 1999},
				new MovieRegexValue(){ Name = "Clueless.1995", ExpectedTitle = "Clueless", ExpectedYear = 1995},
			};


			DirectoryUtility.Setup(x => x.GetDirectoryName(It.IsAny<String>())).Returns<String>(x => x);
			DirectoryUtility.Setup(x => x.Exists(It.IsAny<String>())).Returns(true);
			foreach (var item in files)
			{
				var result = MovieFixer.Parse(item.Name);
				Assert.AreEqual(item.ExpectedTitle, result.Title);
				// ReSharper disable once PossibleInvalidOperationException
				Assert.AreEqual(item.ExpectedYear, result.Year.Value);

			}
		}

		[TestMethod]
		public void Parse_method_should_return_the_movie_title_as_a_title_cased_string()
		{
			var files = new List<MovieRegexValue>
			{
				new MovieRegexValue(){ Name = "Skate.Kitchen.(2018).1080p.WEB-DL.DD5.1.H264-CMRG", ExpectedTitle = "Skate Kitchen", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Christopher.Robin.2018.BDRip.DD5.1.x264-playSD", ExpectedTitle = "Christopher Robin", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "The.Predator.2018.1080p.HC.HDRip.x264.AC3-EVO", ExpectedTitle = "The Predator", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Comedy.Natasha.Leggero.-.Live.at.Bimbos.2015.1080p.WEB-DL.DD+.2.0.x264-TrollHD", ExpectedTitle = "Comedy Natasha Leggero - Live At Bimbos", ExpectedYear = 2015},
				new MovieRegexValue(){ Name = "Beatbox.2015.1080p.AMZN.WEB-DL.DDP5.1.H.264-MZABI", ExpectedTitle = "Beatbox", ExpectedYear = 2015},
				new MovieRegexValue(){ Name = "8.Mile.2002.720p.BluRay.x264-BestHD", ExpectedTitle = "8 Mile", ExpectedYear = 2002},
				new MovieRegexValue(){ Name = "A.Dangerous.Idea.2016.1080p.AMZN.WEB-DL.DDP2.0.H.264-MZABI", ExpectedTitle = "A Dangerous Idea", ExpectedYear = 2016},
				new MovieRegexValue(){ Name = "A.Midsummer.Nights.Dream.2018.HDRip.XviD", ExpectedTitle = "A Midsummer Nights Dream", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Beyond.Clueless.2014.DVDRip.x264-REDBLADE", ExpectedTitle = "Beyond Clueless", ExpectedYear = 2014},
				new MovieRegexValue(){ Name = "Kodachrome.2018.BDRip.XviD", ExpectedTitle = "Kodachrome", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Olafs.Frozen.Adventure.2017.1080p.BluRay.x264-HANDJOB", ExpectedTitle = "Olafs Frozen Adventure", ExpectedYear = 2017},
				new MovieRegexValue(){ Name = "Pepermint.2018.HDRip.XviD", ExpectedTitle = "Pepermint", ExpectedYear = 2018},
				new MovieRegexValue(){ Name = "Kansas.1995.DVDRip.x264-HANDJOB", ExpectedTitle = "Kansas", ExpectedYear = 1995},
				new MovieRegexValue(){ Name = "Gas.Food.Lodging.1992.INTERNAL.BDRip.x264-GHOULS", ExpectedTitle = "Gas Food Lodging", ExpectedYear = 1992},
				new MovieRegexValue(){ Name = "Girl.Interrupted.1999.WEB-DL.720P.H264", ExpectedTitle = "Girl Interrupted", ExpectedYear = 1999},
				new MovieRegexValue(){ Name = "Clueless.1995", ExpectedTitle = "Clueless", ExpectedYear = 1995},
			};


			DirectoryUtility.Setup(x => x.GetDirectoryName(It.IsAny<String>())).Returns<String>(x => x.ToLower());
			DirectoryUtility.Setup(x => x.Exists(It.IsAny<String>())).Returns(true);
			foreach (var item in files)
			{
				var result = MovieFixer.Parse(item.Name);
				Assert.AreEqual(item.ExpectedTitle, result.Title);
				// ReSharper disable once PossibleInvalidOperationException
				Assert.AreEqual(item.ExpectedYear, result.Year.Value);

			}
		}


		#endregion TESTS

		#region PRIVATE MEMBERS


		/// <summary>
		/// 
		/// </summary>
		private class MovieRegexValue
		{
			public String Name { get; set; }
			public String ExpectedTitle { get; set; }
			public Int32 ExpectedYear { get; set; }
		}


		#endregion PRIVATE MEMBERS




	}

}
