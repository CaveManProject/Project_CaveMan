using Godot;

namespace Caveman.Keybind
{

    public partial class HotkeyRebind : Control
    {
        public string action_name = "up";

        public override void _Ready()
        {
            SetActionName();

        }

        private void SetActionName()
        {
            string Label = "unassigned";
            switch (action_name)
            {
                case "up":
                    Label = "UP";
                    break;
                case "down":
                    Label = "DOWN";
                    break;
                case "left":
                    Label = "LEFT";
                    break;
                case "right":
                    Label = "RIGHT";
                    break;
                case "mb_left":
                    Label = "MB LEFT";
                    break;
                case "mb_right":
                    Label = "MB RIGHT";
                    break;
                case "i":
                    Label = "I";
                    break;
                case "e":
                    Label = "E";
                    break;
                case "space":
                    Label = "SPACEBAR";
                    break;
                case "pause":
                    Label = "ESC";
                    break;
                default:
                    break;
            }
        }

        private void SetTextForKey()
        {
            var action_events = InputMap.ActionGetEvents(action_name);
            var action_event = action_events[0];
            if (action_event is InputEventKey)
            {

            }
        }


    }
}