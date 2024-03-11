using Godot;

namespace Caveman.Sound
{
    public partial class SoundController : HSlider
    {
        [Export]
        private string _busName;

        public override void _Ready()
        {
            _ValueChanged(Value);
        }

        private void _ValueChanged(float newValue)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(_busName), newValue);
        }
    }
}
