using Godot;

namespace Caveman.Sound
{
    public partial class Music : HSlider
    {
        public override void _Ready()
        {
            _ValueChanged(Value);
        }

        private void _ValueChanged(float newValue)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), newValue);
        }
    }
}