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

        private void OnOptionButtonItemSelected(int selectedIndex)
        {
            Resolution selectedResolution = (Resolution)selectedIndex;
            Vector2I size = selectedResolution.ToSize();
            DisplayServer.WindowSetSize(size);
        }
    }
}