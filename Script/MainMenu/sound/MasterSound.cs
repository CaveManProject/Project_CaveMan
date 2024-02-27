using Godot;

namespace Caveman.Sound
{
    public partial class MasterSound : HSlider
    {
        public override void _Ready()
        {
            _ValueChanged(Value);
        }

        private void _ValueChanged(float newValue)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), newValue);
        }
    }
}