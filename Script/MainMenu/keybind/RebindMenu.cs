using System.Linq;
using System.Reflection.Metadata;
using Godot;

namespace Caveman.Keybind
{
    public partial class RebindMenu : Control
    {
        private Button button;
        public string actionName = "up";

        public override void _Ready()
        {
            SetProcessUnhandledKeyInput(false);
            SetActionName();
            SetTextForKey();
        }

        private void SetActionName()
        {
            string label = "unassigned";
            label = actionName switch
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
                _ => ""
            };
        }

        private void SetTextForKey()
        {
            var actionEvents = InputMap.ActionGetEvents(actionName);
            if (!actionEvents.Any())
            {
                return;
            }
            var actionEvent = actionEvents[0];
            if (actionEvent is InputEventKey)
            {
                var actionKeycode = OS.GetKeycodeString(((InputEventKey)actionEvent).PhysicalKeycode);
                button.Text = actionKeycode;
            }
            else if (actionEvent is InputEventMouseButton)
            {
                var actionKeycode = OS.GetKeycodeString((Key)((InputEventMouseButton)actionEvent).ButtonIndex);
                button.Text = "Mouse Button " + actionKeycode;
            }
        }

        private void OnButtonToggled(bool buttonPressed)
        {
            if (buttonPressed)
            {
                button.Text = "Press any key.....";
                SetProcessUnhandledKeyInput(buttonPressed);
                foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
                {
                    if (node is RebindMenu)
                    {
                        var menu = node as RebindMenu;
                        if (menu.actionName != this.actionName)
                        {
                            menu.button.ToggleMode = false;
                            menu.SetProcessUnhandledKeyInput(false);
                        }
                    }
                }
            }
            else
            {
                foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
                {
                    if (node is RebindMenu)
                    {
                        var menu = node as RebindMenu;
                        if (menu.actionName != this.actionName)
                        {
                            menu.button.ToggleMode = true;
                            menu.SetProcessUnhandledKeyInput(false);
                        }
                    }
                }
                SetTextForKey();
            }
        }

        private void UnhandledKeyInput(InputEventKey eventKey)
        {
            RebindActionKey(eventKey);
            button.ButtonPressed = false;
        }

        private void RebindActionKey(InputEventKey eventKey)
        {
            var isDuplicate = false;
            var actionEvent = eventKey;
            var actionKeycode = OS.GetKeycodeString(((InputEventKey)actionEvent).PhysicalKeycode);
            foreach (var node in GetTree().GetNodesInGroup("hotkey_button"))
            {
                if (node is RebindMenu)
                {
                    var menu = node as RebindMenu;
                    if (menu.actionName != this.actionName)
                    {
                        if (menu.button.Text == actionKeycode)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                }
            }
            if (!isDuplicate)
            {
                InputMap.ActionEraseEvents(actionName);
                InputMap.ActionAddEvent(actionName, eventKey);
                SetProcessUnhandledKeyInput(false);
                SetTextForKey();
                SetActionName();
            }
        }
    }
}