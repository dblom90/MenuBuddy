using HadoukInput;
using Microsoft.Xna.Framework;
using TouchScreenBuddy;

namespace MenuBuddy
{
	/// <summary>
	/// This is an input helper that uses the hadoukinput to do keyboard/controller input
	/// </summary>
	public class ControllerInputHelper : GameComponent, IInputHelper
	{
		#region Properties

		/// <summary>
		/// This object controls all the controller and keyboard stuff.
		/// </summary>
		public InputState InputState { get; private set; }

		#endregion //Properties

		#region Methods

		public ControllerInputHelper(Game game)
			: base(game)
		{
			InputState = new InputState();

			//Register ourselves to implement the DI container service.
			game.Components.Add(this);
			game.Services.AddService(typeof(IInputHelper), this);
		}

		public void Update()
		{
			//Read the keyboard and gamepad.
			InputState.Update();
		}

		public void HandleInput(IScreen screen)
		{
			//check if this is the game screen
			var game = screen as IGameScreen;
			if (null != game)
			{
				game.HandleInput(InputState);
			}

			//check if is a menu screen
			var menu = screen as IMenuScreen;
			if (null != menu)
			{
				//Check all the input
				if (InputState.IsMenuUp(screen.ControllingPlayer))
				{
					// Move to the previous menu entry
					menu.MenuUp();
				}
				else if (InputState.IsMenuDown(screen.ControllingPlayer))
				{
					// Move to the next menu entry
					menu.MenuDown();
				}

				//checkl the left/right messages
				if (InputState.IsMenuLeft(screen.ControllingPlayer))
				{
					//send a left message to the current menu entry
					menu.MenuLeft();
				}
				else if (InputState.IsMenuRight(screen.ControllingPlayer))
				{
					//send a right message to the current menu entry
					menu.MenuRight();
				}

				// Accept or cancel the menu? We pass in our ControllingPlayer, which may
				// either be null (to accept input from any player) or a specific index.
				// If we pass a null controlling player, the InputState helper returns to
				// us which player actually provided the input. We pass that through to
				// OnSelectEntry and OnCancel, so they can tell which player triggered them.
				PlayerIndex playerIndex;

				if (InputState.IsMenuSelect(screen.ControllingPlayer, out playerIndex))
				{
					menu.OnSelect(playerIndex);
				}
				else if (InputState.IsMenuCancel(screen.ControllingPlayer, out playerIndex))
				{
					menu.OnCancel(playerIndex);
				}
			}
		}

		#endregion //Methods
	}
}