using Godot;

namespace Caveman.UI
{
	public partial class ProgressBarsUI : Control
	{
		public TextureProgressBar healthBar;
		public TextureProgressBar staminaBar;

		public override void _Ready()
		{
			healthBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/HealthBar");
			staminaBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/StaminaBar");
		}

		public override void _Process(double delta)
		{
			if (this.GetTree().Paused && this.Visible)
			{
				this.Hide();
			}
			if (!this.GetTree().Paused && !this.Visible)
			{
				this.Show();
			}
		}
		public void ModifyStamina(double amount)
		{
			staminaBar.Value = (float)amount;
		}

		public void ModifyHealth(double amount)
		{
			healthBar.Value = (float)amount;
		}
	}
}
