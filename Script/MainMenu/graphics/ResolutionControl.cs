using System;
using Caveman.Enums;
using Godot;

namespace Caveman.Graphics
{
    public partial class ResolutionControl : Control
    {
        private OptionButton optionButton;
        public override void _Ready()
        {
            optionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");
            foreach (Resolution resolution in Enum.GetValues(typeof(Resolution)))
            {
                optionButton.AddItem(resolution.ToDisplayString());
            }
        }

        private void OnOptionButtonItemSelected(Resolution index)
        {
            switch (index)
            {
                case Resolution.R_1152x648:
                    DisplayServer.WindowSetSize(new Vector2I(1152, 648));
                    break;
                case Resolution.R_640x800:
                    DisplayServer.WindowSetSize(new Vector2I(640, 800));
                    break;
                case Resolution.R_1280x720:
                    DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                    break;
                case Resolution.R_1600x900:
                    DisplayServer.WindowSetSize(new Vector2I(1600, 900));
                    break;
                case Resolution.R_2560x1440:
                    DisplayServer.WindowSetSize(new Vector2I(2560, 1440));
                    break;
                case Resolution.R_3840x2160:
                    DisplayServer.WindowSetSize(new Vector2I(3840, 2160));
                    break;
            }
        }
    }
}