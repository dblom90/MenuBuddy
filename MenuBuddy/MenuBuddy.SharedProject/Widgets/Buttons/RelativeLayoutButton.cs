using Microsoft.Xna.Framework;

namespace MenuBuddy
{
	/// <summary>
	/// This is a button thhat contains a relaitve layout
	/// </summary>
	public class RelativeLayoutButton : LayoutButton<RelativeLayout>
	{
		#region Initialization

		/// <summary>
		/// Constructs a new menu entry with the specified text.
		/// </summary>
		public RelativeLayoutButton()
		{
			Layout = new RelativeLayout();
		}

		public RelativeLayoutButton(RelativeLayoutButton inst) : base(inst)
		{
			Layout = new RelativeLayout(inst.Layout as RelativeLayout);
		}

		/// <summary>
		/// Get a deep copy of this item
		/// </summary>
		/// <returns></returns>
		public override IScreenItem DeepCopy()
		{
			return new RelativeLayoutButton(this);
		}

		#endregion //Initialization

		#region Methods

		protected override void CalculateRect()
		{
			//get the size of the rect
			var size = Size * Scale;

			//set the x component
			Vector2 pos = Position.ToVector2();
			switch (Horizontal)
			{
				case HorizontalAlignment.Center: { pos.X -= size.X / 2f; } break;
				case HorizontalAlignment.Right: { pos.X -= size.X; } break;
			}

			//set the y component
			switch (Vertical)
			{
				case VerticalAlignment.Center: { pos.Y -= size.Y / 2f; } break;
				case VerticalAlignment.Bottom: { pos.Y -= size.Y; } break;
			}

			_rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

			//Set the position of the internal layout
			var relLayout = Layout as RelativeLayout;
			if (null != relLayout)
			{
				var layoutSize = size - ((Padding * 2f) * Scale);
				var layoutPos = pos + (Padding * Scale);
				relLayout.Scale = Scale;
				relLayout.Vertical = VerticalAlignment.Top;
				relLayout.Horizontal = HorizontalAlignment.Left;
				relLayout.Position = layoutPos.ToPoint();
				relLayout.Size = layoutSize;
			}
		}

		#endregion
	}
}