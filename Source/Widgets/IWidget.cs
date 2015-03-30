using System;
using FontBuddyLib;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MenuBuddy
{
	/// <summary>
	/// Interface for the widget object.
	/// A widget is a screen item that is displayed on the page
	/// </summary>
	public interface IWidget : IScreenItem
	{
		/// <summary>
		/// The stylesheet of this item
		/// </summary>
		StyleSheet Style { get; }

		/// <summary>
		/// horizontal alignment of this item
		/// </summary>
		HorizontalAlignment Horizontal { get; set; }

		/// <summary>
		/// vertical alignment of this item
		/// </summary>
		VerticalAlignment Vertical { get; set; }
	}
}