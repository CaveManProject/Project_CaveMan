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
        private void OnOptionButtonItemSelected(FpsMode index)
        {
            switch (index)
            {
                case FpsMode.FPS_30:
                    Engine.MaxFps = 30;
                    break;
                case FpsMode.FPS_60:
                    Engine.MaxFps = 60;
                    break;
                case FpsMode.FPS_120:
                    Engine.MaxFps = 120;
                    break;
                case FpsMode.FPS_144:
                    Engine.MaxFps = 144;
                    break;
                case FpsMode.FPS_240:
                    Engine.MaxFps = 240;
                    break;
                case FpsMode.FPS_unlimited:
                    Engine.MaxFps = 0;
                    break;
            }
        }
    }
}