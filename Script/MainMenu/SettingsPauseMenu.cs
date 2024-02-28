using Godot;

namespace Caveman.Menu
{
	public partial class SettingsPauseMenu : Control
	{
		private AudioStreamPlayer2D _uiClick;

		public override void _Ready()
		{
			this._uiClick = this.GetNode<AudioStreamPlayer2D>("UIClick");
		}

		private void OnResumePressed()
		{
			_uiClick.Play();
			this.GetTree().ChangeSceneToFile("res://Scenes/World/world.tscn");
		}
	}
}
