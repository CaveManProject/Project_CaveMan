using System;
using Godot;

namespace Caveman.Menu
{
	public partial class MainMenu : Control
	{
		private AudioStreamPlayer2D ui_click;
		private MarginContainer margin_container;

		private Control settings_menu;
		private Control madeby_menu;

		private PackedScene start_lvl = GD.Load<PackedScene>("res://Scenes/world.tscn");

		public override void _Ready()
		{
			this.ui_click = this.GetNode<AudioStreamPlayer2D>("ui_click");
			this.margin_container = this.GetNode<MarginContainer>("MarginContainer");
			this.settings_menu = this.GetNode<Control>("SettingsMenu");
			this.madeby_menu = this.GetNode<Control>("madeby_menu");
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
			settings_menu.Show();
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
	}
}
