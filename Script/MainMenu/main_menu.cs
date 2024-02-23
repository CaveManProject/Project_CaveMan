using System;
using Godot;

//amespace Caveman
//{
    public partial class   MainMenu : Control
    {
        private TextureButton start_button;
        private TextureButton settings_button;
        private TextureButton exit_button;
        private TextureButton madeby_button;

        private AudioStreamPlayer2D ui_click;
        private MarginContainer margin_container;

        private SettingsMenu settings_menu;
        private MadeByMenu madeby_menu;

        private PackedScene start_lvl;

        public override void _Ready()
        {
            HandleConnectionSignals();
        }

        private void OnStartPressed()
        {
            ui_click.Play();
            GetTree().ChangeSceneToPacked(start_lvl);
        }

        private void OnSettingsPressed()
        {
            ui_click.Play();
            margin_container.Hide();
            settings_menu.SetProcess(true);
            settings_menu.show();
        }

        private void OnMadeByPressed()
        {
            ui_click.Play();
            margin_container.Hide();
            madeby_menu.SetProcess(true);
            madeby_menu.Show();
        }

        private void OnExitPressed()
        {
            ui_click.Play();
            GetTree().Quit();
        }

     private void OnExitSettingsMenu()
        {
            margin_container.Show();
            settings_menu.Hide();
         }       

        private void OnExitMadeByMenu()
        {
            margin_container.Show();
            madeby_menu.Hide();
        }

        private void HandleConnectionSignals()
        {
           start_button.Connect("pressed", this, nameof(OnStartPressed));
        settings_button.Connect("pressed", this, nameof(OnSettingsPressed));
        madeby_button.Connect("pressed", this, nameof(OnMadeByPressed));
        exit_button.Connect("pressed", this, nameof(OnExitPressed));
        settings_menu.Connect("exit_settings_menu", this, nameof(OnExitSettingsMenu));
        madeby_menu.Connect("exit_madeby_menu", this, nameof(OnExitMadeByMenu));
        }

        private uint nameof(Action onStartPressed)
        {
            throw new NotImplementedException();
        }
    }  
//}