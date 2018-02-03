﻿using FontBuddyLib;
using MenuBuddy;
using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuddy.Tests
{
	[TestFixture]
	public class SliderTests
	{
		#region Fields

		private Mock<IFontBuddy> _font;
		private Slider _slider;

		#endregion //Fields

		#region Setup

		[SetUp]
		public void LabelTests_Setup()
		{
			_font = new Mock<IFontBuddy>() { CallBase = true };
			_font.Setup(x => x.MeasureString(It.IsAny<string>()))
				.Returns(new Vector2(30f, 40f));

			_slider = new Slider();
		}

		#endregion //Setup

		#region Tests

		[Test]
		public void SliderMinPos()
		{
			_slider = new Slider()
			{
				Min = 1,
				Max = 5
			};

			_slider.SliderPosition = 0;

			Assert.AreEqual(1f, _slider.SliderPosition);
		}

		[Test]
		public void SliderMaxPos()
		{
			_slider = new Slider()
			{
				Min = 1,
				Max = 5
			};

			_slider.SliderPosition = 10f;

			Assert.AreEqual(5f, _slider.SliderPosition);
		}

		[Test]
		public void SliderOkPos()
		{
			_slider = new Slider()
			{
				Min = 1,
				Max = 5
			};

			_slider.SliderPosition = 3f;

			Assert.AreEqual(3f, _slider.SliderPosition);
		}

		[Test]
		public void SliderPos1()
		{
			var min1 = 1f;
			var max1 = 5f;
			var pos1 = 3f;
			var min2 = 200f;
			var max2 = 600f;

			Assert.AreEqual(400f, Slider.SolveSliderPos(min1, max1, pos1, min2, max2));
		}

		[Test]
		public void SliderPos2()
		{
			var min1 = 200f;
			var max1 = 600f;
			var pos1 = 400f;
			var min2 = 1f;
			var max2 = 5f;

			Assert.AreEqual(3f, Slider.SolveSliderPos(min1, max1, pos1, min2, max2));
		}

		#endregion //Tests
	}
}
