using Godot;

namespace Caveman.Menu
{
    public partial class PauseMenu : Control
    {
        private AudioStreamPlayer2D _uiClick;
        private PackedScene _settingsScene = GD.Load<PackedScene>("res://Scenes/MainMenu/settings_pause_menu.tscn");
        public override void _Ready()
        {
            this._uiClick = this.GetNode<AudioStreamPlayer2D>("ui_click");
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("pause") && !this.GetTree().Paused)
            {
                this.Pause();
            }
            else if (Input.IsActionJustReleased("pause") && this.GetTree().Paused)
            {
                this.Resume();
            }
        }

        private void Resume()
        {
            this.GetTree().Paused = false;
            this.Hide();
        }

        private void Pause()
        {
            this.GetTree().Paused = true;
            this.Show();
        }

        private void _on_resume_pressed()
        {
            Resume();
        }

        private void _on_settings_pressed()
        {
            this.GetTree().Paused = false;
            this.GetTree().ChangeSceneToPacked(_settingsScene);
        }

        private void _on_quit_pressed()
        {
            this.GetTree().Paused = false;
            this.GetTree().ChangeSceneToFile("res://Scenes/MainMenu/main_menu.tscn");
        }

    }
}