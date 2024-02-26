using Godot;

namespace Caveman.Menu
{
    public partial class SettingsMenu : Control
    {
        private AudioStreamPlayer2D _uiClick;


        public override void _Ready()
        {
            this._uiClick = this.GetNode<AudioStreamPlayer2D>("ui_click");
        }

        private void _on_back_pressed()
        {
            this.GetTree().ChangeSceneToFile("res://Scenes/MainMenu/main_menu.tscn");
        }
    }
}
