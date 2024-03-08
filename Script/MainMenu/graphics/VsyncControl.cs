using Godot;
using static Godot.DisplayServer;

namespace Caveman.Graphics
{
	public partial class VsyncControl : Control
	{
		private CheckButton ON_OFF;

		public override void _Ready()
		{
			ON_OFF = GetNode<CheckButton>("HBoxContainer/ON_OFF");
		}

		private void OnCheckButtonToggled(bool ToggleOn)
		{
			if (ToggleOn == true)
			{
				WindowSetVsyncMode(VSyncMode.Enabled);
			}
			else
			{
				WindowSetVsyncMode(VSyncMode.Disabled);
			}
		}
	}
}
