using System;
using Godot;

namespace Caveman.Menu
{
	public partial class MainMenu : Control
	{
		private AudioStreamPlayer2D _uiClick;
		private MarginContainer _marginContainer;

		private Control _settingsMenu;
		private Control _madeByMenu;

		private PackedScene start_lvl = GD.Load<PackedScene>("res://Scenes/World/world.tscn");

		public override void _Ready()
		{
			this._uiClick = this.GetNode<AudioStreamPlayer2D>("UIClick");
			this._marginContainer = this.GetNode<MarginContainer>("MarginContainer");
			this._settingsMenu = this.GetNode<Control>("SettingsMenu");
			this._madeByMenu = this.GetNode<Control>("MadeByMenu");
		}

		private void OnStartPressed()
		{
			_uiClick.Play();
			GetTree().ChangeSceneToPacked(start_lvl);
		}

		private void OnSettingsPressed()
		{
			_uiClick.Play();
			_marginContainer.Hide();
			_settingsMenu.SetProcess(true);
			_settingsMenu.Show();
		}

		private void OnMadeByPressed()
		{
			_uiClick.Play();
			_marginContainer.Hide();
			_madeByMenu.SetProcess(true);
			_madeByMenu.Show();
		}

		private void OnExitPressed()
		{
			_uiClick.Play();
			GetTree().Quit();
		}

		private void OnExitSettingsMenu()
		{
			_marginContainer.Show();
			_settingsMenu.Hide();
		}

		private void OnExitMadeByMenu()
		{
			_marginContainer.Show();
			_madeByMenu.Hide();
		}
	}
}
