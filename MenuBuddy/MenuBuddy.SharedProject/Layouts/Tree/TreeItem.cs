using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MenuBuddy
{
	public class TreeItem<T> : StackLayout
	{
		#region Fields

		/// <summary>
		/// the tree item that this guy is underneath
		/// null if it is a top level node
		/// </summary>
		TreeItem<T> _parent;

		/// <summary>
		/// how many tabs in this guy is
		/// </summary>
		int _indentation;

		Image ExpandCollapseImage
		{
			get; set;
		}

		bool _expanded;
		public bool Expanded
		{
			get
			{
				return _expanded;
			}
		}

		Texture2D _expandTexture;
		Texture2D _collapseTexture;

		#endregion //Fields

		#region Properties

		/// <summary>
		/// The item this dropdownitem is representing in the gui
		/// </summary>
		public T Item
		{
			get; set;
		}

		/// <summary>
		/// the item this guy is managing
		/// </summary>
		protected RelativeLayoutButton ItemButton
		{
			get;
			set;
		}

		public List<TreeItem<T>> ChildItems
		{
			get; private set;
		}

		/// <summary>
		/// The tree control that owns this dude
		/// </summary>
		protected ITree<T> Tree { get; set; }

		#endregion //Properties

		#region Initialization

		public TreeItem(T item, ITree<T> tree, TreeItem<T> parent)
		{
			Item = item;
			
			Alignment = StackAlignment.Left;
			Horizontal = HorizontalAlignment.Left;
			Vertical = VerticalAlignment.Center;
			Tree = tree;
			_parent = parent;
			_indentation = (parent == null ? 0 : parent._indentation + 1);
			ChildItems = new List<TreeItem<T>>();
			_expanded = false;

			ItemButton = new RelativeLayoutButton();
			ItemButton.OnClick += ((obj, e) =>
			{
				tree.SelectedTreeItem = this;
			});
		}

		public TreeItem(TreeItem<T> inst) : base(inst)
		{
			Item = inst.Item;
			ItemButton = new RelativeLayoutButton(inst.ItemButton);
			Tree = inst.Tree;
			_parent = inst._parent;
			_indentation = inst._indentation;
			_expanded = inst._expanded;
			_expandTexture = inst._expandTexture;
			_collapseTexture = inst._collapseTexture;
			ExpandCollapseImage = new Image(inst.ExpandCollapseImage);

			ChildItems = new List<TreeItem<T>>();
			foreach (var item in inst.ChildItems)
			{
				var treeItem = item.DeepCopy() as TreeItem<T>;
				if (null != treeItem)
				{
					ChildItems.Add(treeItem);
				}
			}
		}

		public override IScreenItem DeepCopy()
		{
			return new TreeItem<T>(this);
		}

		private void SetScreen(IScreen screen)
		{
			if (null != screen.ScreenManager)
			{
				if (null == _expandTexture)
				{
					_expandTexture = screen.ScreenManager.Game.Content.Load<Texture2D>(StyleSheet.TreeExpandImageResource);
				}

				if (null == _collapseTexture)
				{
					_collapseTexture = screen.ScreenManager.Game.Content.Load<Texture2D>(StyleSheet.TreeCollapseImageResource);
				}
			}
		}

		#endregion //Initialization

		#region Methods

		public void SetButton(IScreenItem item)
		{
			ItemButton.Size = new Vector2(item.Rect.Width, item.Rect.Height * .75f);
			ItemButton.AddItem(item);
		}

		public override void AddItem(IScreenItem item)
		{
			var childItem = item as TreeItem<T>;
			if (childItem != null)
			{
				//if this is child tree item, add it special
				ChildItems.Add(childItem);
			}
			else
			{
				base.AddItem(item);
			}
		}

		/// <summary>
		/// This method gets called when the parent node is expanded
		/// </summary>
		public void AddToTree(IScreen screen)
		{
			Items.Clear();

			SetScreen(screen);

			_expanded = false;

			//get the size of the brick to add for tabs
			var tab = new Vector2(ItemButton.Rect.Height, ItemButton.Rect.Height) * .5f;

			//Add a brick for each tab
			for (int i = 0; i < _indentation; i++)
			{
				AddItem(new Shim()
				{
					Size = tab,
					Horizontal = HorizontalAlignment.Left,
					Vertical = VerticalAlignment.Center,
				});
			}

			//if there are any sub-items, need to add the expand/collapse button
			if (ChildItems.Count > 0)
			{
				//add the expansion button
				ExpandCollapseImage = new Image()
				{
					Texture = _expandTexture,
					Size = tab,
					Horizontal = HorizontalAlignment.Center,
					Vertical = VerticalAlignment.Center,
					FillRect = true,
				};

				//create a stack layout to size teh button correctly
				var expandButton = new RelativeLayoutButton()
				{
					Size = tab * 2f,
					Horizontal = HorizontalAlignment.Center,
					Vertical = VerticalAlignment.Center,
				};
				expandButton.AddItem(ExpandCollapseImage);
				expandButton.OnClick += ToggleExpansion;

				AddItem(expandButton);
			}
			else
			{
				AddItem(new Shim()
				{
					Size = tab * 2f,
				});
			}

			//add that final button
			AddItem(ItemButton);
		}

		public void ToggleExpansion(object obj, ClickEventArgs e)
		{
			//call the correct method to expand/collapse the list
			if (_expanded)
			{
				Collapse(obj, e);
			}
			else
			{
				Expand(obj, e);
			}

			//toggle that flag
			_expanded = !_expanded;
		}

		/// <summary>
		/// This method gets called when this node is expanded
		/// </summary>
		private void Expand(object obj, ClickEventArgs e)
		{
			//add all thechild items in reverse order
			for (int i = ChildItems.Count - 1; i >= 0; i--)
			{
				//Add this whole thing to the main control
				Tree.InsertItem(ChildItems[i], this);
			}

			//swap out the image of the expand/collapse button
			if (null != ExpandCollapseImage)
			{
				ExpandCollapseImage.Texture = _collapseTexture;
			}
		}

		/// <summary>
		/// this method gets called when this node is collapsed
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="e"></param>
		private void Collapse(object obj, ClickEventArgs e)
		{
			//recurse into child items to remove everything under this guy from the tree
			foreach (var item in ChildItems)
			{
				item.Collapse(obj, e);
				Tree.RemoveItem(item);
			}

			//swap out the image of the expand/collapse button
			if (null != ExpandCollapseImage)
			{
				ExpandCollapseImage.Texture = _expandTexture;
			}
		}

		#endregion //Methods
	}
}