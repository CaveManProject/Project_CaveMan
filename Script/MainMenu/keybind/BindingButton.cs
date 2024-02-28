using System.Linq;
using Godot;

namespace Caveman.Keybind
{
	public partial class BindingButton : Control
	{
		[Export]
		public string actionName;
		public Button button;
		private Label _label;

		/// <summary>
		/// Called when the node enters the scene tree for the first time.
		/// </summary>
		public override void _Ready()
		{
			button = GetNode<Button>("HBoxContainer/Button");
			_label = GetNode<Label>("HBoxContainer/action_button");
			SetLabelText();
			SetTextForKey();
		}

		/// <summary>
		/// Set the text of the label to the readable action name
		/// </summary>
		private void SetLabelText()
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

		/// <summary>
		/// Set the text of the button to the key assigned to the action
		/// </summary>
		private void SetTextForKey()
		{
			var actionEvents = InputMap.ActionGetEvents(actionName);
			if (!actionEvents.Any())
			{
				return;
			}
			if (actionEvents[0] is InputEventKey keyEvent)
			{
				var actionKeycode = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
				button.Text = actionKeycode;
			}
			else if (actionEvents[0] is InputEventMouseButton button)
			{
				this.button.Text = "MB " + button.ButtonIndex;
			}
		}

		/// <summary>
		/// Event handler: Set the button to toggle mode and enable the process unhandled key input
		/// </summary>
		private void OnButtonToggled(bool buttonPressed)
		{
			if (buttonPressed)
			{
				button.Text = "Press any key.....";
				SetProcessUnhandledInput(true);
				// Disable other buttons
				foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
				{
					if (node is BindingButton bindingButton)
					{
						if (bindingButton.actionName != this.actionName)
						{
							bindingButton.button.ToggleMode = false;
							bindingButton.SetProcessUnhandledInput(false);
						}
					}
				}
			}
		}

		/// <summary>
		/// Event handler: Rebind the action key
		/// </summary>
		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event is InputEventKey keyEvent)
			{
				if (keyEvent.Keycode == Key.Escape)
				{
					// Enable other buttons
					foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
					{
						if (node is BindingButton bindingButton)
						{
							if (bindingButton.actionName != this.actionName)
							{
								bindingButton.button.ToggleMode = true;
								bindingButton.SetProcessUnhandledInput(false);
							}
						}
					}
					this.button.ToggleMode = true;
					this.SetProcessUnhandledInput(false);
					SetTextForKey();
					return;
				}
				SetProcessUnhandledInput(false);
				RebindActionKey(keyEvent);
				button.ButtonPressed = false;
			}
			else if (@event is InputEventMouseButton mouseButton)
			{
				SetProcessUnhandledInput(false);
				RebindActionMouseButton(mouseButton);
				button.ButtonPressed = false;
			}
		}

		/// <summary>
		/// Rebind the action key if the key is not already assigned to another action
		/// </summary>
		/// <param name="keyEvent">Pressed key event</param>
		private void RebindActionKey(InputEventKey keyEvent)
		{
			var actionKeycode = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
			// Check if the key is already assigned to another action
			foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
			{
				if (node is BindingButton menu)
				{
					if (menu.actionName != this.actionName && menu.button.Text == actionKeycode)
					{
						this.button.ToggleMode = true;
						this.SetProcessUnhandledInput(false);
						SetTextForKey();
						GD.Print("Key already assigned to another action");
						return;
					}
				}
			}
			// If the key is not assigned to another action, rebind the action
			InputMap.ActionEraseEvents(actionName);
			InputMap.ActionAddEvent(actionName, keyEvent);
			SetTextForKey();
		}

		/// <summary>
		/// Rebind the action mouse button if the button is not already assigned to another action
		/// </summary>
		/// <param name="mouseButton">Pressed mouse button event</param>
		private void RebindActionMouseButton(InputEventMouseButton mouseButton)
		{
			// Check if the mouse button is already assigned to another action
			foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
			{
				if (node is BindingButton menu)
				{
					if (menu.actionName != this.actionName && menu.button.Text == "MB " + mouseButton.ButtonIndex)
					{
						this.button.ToggleMode = true;
						this.SetProcessUnhandledInput(false);
						SetTextForKey();
						GD.Print("Mouse button already assigned to another action: ", menu.actionName);
						return;
					}
				}
			}
			// If the mouse button is not assigned to another action, rebind the action
			InputMap.ActionEraseEvents(actionName);
			InputMap.ActionAddEvent(actionName, mouseButton);
			SetTextForKey();
		}
	}
}
