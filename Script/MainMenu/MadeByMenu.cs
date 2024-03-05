using Godot;

namespace Caveman.Menu
{
	public partial class MadeByMenu : Control
	{
		private AudioStreamPlayer2D _uiClick;

		public override void _Ready()
		{
			this._uiClick = this.GetNode<AudioStreamPlayer2D>("UIClick");
		}

		private void OnBackPressed()
		{
			_uiClick.Play();
			this.GetTree().ChangeSceneToFile("res://Scenes/MainMenu/main_menu.tscn");
		}
	}
}
