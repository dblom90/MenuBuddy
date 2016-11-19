using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuBuddy
{
	/// <summary>
	/// Background image is an image that is drawn behind all the button backgrounds and screen items
	/// </summary>
	public class BackgroundImage : Image
	{
		#region Initialization

		/// <summary>
		/// constructor!
		/// </summary>
		public BackgroundImage(Texture2D texture)
			: base(texture)
		{
			PulsateOnHighlight = false;
        }

		public BackgroundImage(BackgroundImage inst) : base(inst)
		{
		}

		#endregion //Initialization

		#region Methods

		public override void DrawBackground(IScreen screen, GameClock gameTime)
		{
			//Draw the image as the background item
			base.Draw(screen, gameTime);
		}

		public override void Draw(IScreen screen, GameClock gameTime)
		{
			//Do nothing when this item is asked to "draw"
		}

		#endregion //Methods
	}
}