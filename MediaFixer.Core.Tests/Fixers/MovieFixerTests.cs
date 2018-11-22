using System;
using System.Collections.Generic;
using System.IO;
using MediaFixer.Core.Configuration;
using MediaFixer.Core.Fixers;
using MediaFixer.Core.IO;
using MediaFixer.Core.Logging;
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
			Settings.Setup(x => x.MovieRegex).Returns(@"(?<title>.*)(?<year>19|20\d{2})");
			Settings.Setup(x => x.MovieYearRegex).Returns(@"(19|20)\d{2}");
			MovieFixer = new MovieFixer(Settings.Object, Logger.Object, DirectoryUtility.Object, FileUtility.Object);
		}


		#endregion SETUP

		#region TESTS


		[TestMethod]
		public void Parse_method_should_correctly_identify_movie_parts()
		{
			var files = new List<MovieRegexValue>
			{
				new MovieRegexValue() {Name = "Skate.Kitchen.2018.1080p.WEB-DL.DD5.1.H264-CMRG", ExpectedTitle = "Skate Kitchen", ExpectedYear = 2018},
				new MovieRegexValue() {Name = "Christopher.Robin.2018.BDRip.DD5.1.x264-playSD", ExpectedTitle = "Christopher Robin", ExpectedYear = 2018},
				new MovieRegexValue() {Name = "The.Predator.2018.1080p.HC.HDRip.x264.AC3-EVO", ExpectedTitle = "The Predator", ExpectedYear = 2018},
				new MovieRegexValue() {Name = "Comedy.Natasha.Leggero.-.Live.at.Bimbos.2015.1080p.WEB-DL.DD+.2.0.x264-TrollHD", ExpectedTitle = "Comedy Natasha Leggero - Live at Bimbos", ExpectedYear = 2015},
				new MovieRegexValue() {Name = "Beatbox.2015.1080p.AMZN.WEB-DL.DDP5.1.H.264-MZABI", ExpectedTitle = "Beatbox", ExpectedYear = 2015},
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



		#endregion TESTS

		#region PRIVATE MEMBERS


		private class MovieRegexValue
		{
			public String Name { get; set; }
			public String ExpectedTitle { get; set; }
			public Int32 ExpectedYear { get; set; }
		}

		#endregion PRIVATE MEMBERS




	}

}
