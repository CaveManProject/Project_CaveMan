using System.Linq;
using System.Reflection.Metadata;
using Godot;

namespace Caveman.Keybind
{
	public partial class RebindMenu : Control
	{
		private Button _button;
		private Label _label;

		[Export]
		public string actionName;

		public override void _Ready()
		{
			_button = GetNode<Button>("HBoxContainer/Button");
			_label = GetNode<Label>("HBoxContainer/action_button");
			SetProcessUnhandledKeyInput(false);
			SetActionName();
			SetTextForKey();
		}

		private void SetActionName()
		{
			_label.Text = actionName switch
			{
				"up" => "UP",
				"down" => "DOWN",
				"left" => "LEFT",
				"right" => "RIGHT",
				"mb_left" => "MB LEFT",
				"mb_right" => "MB RIGHT",
				"i" => "I",
				"e" => "E",
				"space" => "SPACEBAR",
				"pause" => "ESC",
				_ => "unassigned"
			};
		}

		private void SetTextForKey()
		{
			var actionEvents = InputMap.ActionGetEvents(actionName);
			if (!actionEvents.Any())
			{
				return;
			}
			if (actionEvents[0] is InputEventKey keyEvent)
			{
				GD.Print(keyEvent.PhysicalKeycode);
				var actionKeycode = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
				_button.Text = actionKeycode;
			}
			else if (actionEvents[0] is InputEventMouseButton button)
			{
				var actionKeycode = OS.GetKeycodeString((Key)button.ButtonIndex);
				_button.Text = "Mouse Button " + actionKeycode;
			}
		}

		private void OnButtonToggled(bool buttonPressed)
		{
			GD.Print("Button Pressed: " + buttonPressed);
			if (buttonPressed)
			{
				_button.Text = "Press any key.....";
				SetProcessUnhandledKeyInput(buttonPressed);
				// Disable other buttons
				foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
				{
					if (node is RebindMenu menu)
					{
						if (menu.actionName != this.actionName)
						{
							menu._button.ToggleMode = false;
							menu.SetProcessUnhandledKeyInput(false);
						}
					}
				}
			}
			else
			{
				// Enable other buttons
				foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
				{
					if (node is RebindMenu menu)
					{
						if (menu.actionName != this.actionName)
						{
							menu._button.ToggleMode = true;
							menu.SetProcessUnhandledKeyInput(false);
						}
					}
				}
				SetTextForKey();
			}
		}

		public override void _UnhandledKeyInput(InputEvent @event)
		{
			if (@event is InputEventKey keyEvent)
			{
				GD.Print("Unhandled Key Input: " + keyEvent.Keycode);
				RebindActionKey(keyEvent);
				_button.ButtonPressed = false;
			}
		}

		private void RebindActionKey(InputEventKey keyEvent)
		{
			var actionKeycode = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
			// Check if the key is already assigned to another action
			foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
			{
				if (node is RebindMenu menu)
				{
					if (menu.actionName != this.actionName && menu._button.Text == actionKeycode)
					{
						GD.Print("Key already assigned to another action");
						return;
					}
				}
			}
			// If the key is not assigned to another action, rebind the action
			InputMap.ActionEraseEvents(actionName);
			InputMap.ActionAddEvent(actionName, keyEvent);
			SetProcessUnhandledKeyInput(false);
			SetTextForKey();
			SetActionName();

		}
	}
}
