using System;
using Caveman.Enums;
using Godot;

namespace Caveman.Menu.Graphics
{
	public partial class WindowControl : Control
	{
		private OptionButton _optionButton;

		public override void _Ready()
		{
			_optionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");
			foreach (WindowMode windowMode in Enum.GetValues(typeof(WindowMode)))
			{
				_optionButton.AddItem(windowMode.ToDisplayString());
			}
		}

		private void OnOptionButtonItemSelected(WindowMode index)
		{
			switch (index)
			{
				case WindowMode.FullScreen:
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
					DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
					break;
				case WindowMode.Windowed:
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
					DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
					break;
				case WindowMode.Borderless:
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
					DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
					break;
			}
		}
	}
}

