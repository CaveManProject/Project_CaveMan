using System;
using Caveman.Enums;
using Godot;
using static Godot.DisplayServer;

namespace Caveman.Graphics
{
    public partial class GraphicsController : VBoxContainer
    {
        public override void _Ready()
        {
            var fpsDropdown = GetNode<OptionButton>("FPSContainer/FPSDropdown");
            foreach (FpsMode fps in Enum.GetValues(typeof(FpsMode)))
            {
                fpsDropdown.AddItem(fps.ToDisplayString());
            }

            var resolutionDropdown = GetNode<OptionButton>("ResolutionContainer/ResolutionDropdown");
            foreach (Resolution resolution in Enum.GetValues(typeof(Resolution)))
            {
                resolutionDropdown.AddItem(resolution.ToDisplayString());
            }

            var vsyncSwitch = GetNode<CheckButton>("VSyncContainer/VSyncSwitch");

            var windowModeDropdown = GetNode<OptionButton>("WindowModeContainer/WindowModeDropdown");
            foreach (Enums.WindowMode windowMode in Enum.GetValues(typeof(Enums.WindowMode)))
            {
                windowModeDropdown.AddItem(windowMode.ToDisplayString());
            }
        }

        private void OnFPSDropdownOptionButtonItemSelected(int selectedIndex)
        {
            FpsMode selectedFpsMode = (FpsMode)selectedIndex;
            int fpsLimit = selectedFpsMode.ToFpsLimit();
            Engine.MaxFps = fpsLimit;
        }

        private void OnResolutionDropdownOptionButtonItemSelected(int selectedIndex)
        {
            Resolution selectedResolution = (Resolution)selectedIndex;
            Vector2I size = selectedResolution.ToSize();
            DisplayServer.WindowSetSize(size);
        }

        private void OnVSyncSwitchCheckButtonToggled(bool ToggleOn)
        {
            if (ToggleOn == true)
            {
                WindowSetVsyncMode(VSyncMode.Enabled);
                GD.Print("ON");
            }
            else
            {
                WindowSetVsyncMode(VSyncMode.Disabled);
            }
        }

        private void OnWindowModeDropdownOptionButtonItemSelected(Enums.WindowMode index)
        {
            switch (index)
            {
                case Enums.WindowMode.FullScreen:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                    break;
                case Enums.WindowMode.Windowed:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                    break;
                case Enums.WindowMode.Borderless:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                    break;
            }
        }
    }
}