using System;
using Caveman.Enums;
using Godot;

namespace Caveman.Graphics
{
    public partial class FpsControl : Control
    {
        private OptionButton OptionButton;
        public override void _Ready()
        {
            OptionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");
            foreach (FpsMode fps in Enum.GetValues(typeof(FpsMode)))
            {
                OptionButton.AddItem(fps.ToDisplayString());
            }
        }
        private void OnOptionButtonItemSelected(int selectedIndex)
        {
            FpsMode selectedFpsMode = (FpsMode)selectedIndex;
            int fpsLimit = selectedFpsMode.ToFpsLimit();
            Engine.MaxFps = fpsLimit;
        }
    }
}