using Godot;

namespace Caveman.Enums
{
    public enum FpsMode
    {
        FPS_30,
        FPS_60,
        FPS_120,
        FPS_144,
        FPS_240,
        FPS_unlimited
    }
    public static class FpsExtensions
    {
        public static string ToDisplayString(this FpsMode fps)
        {
            return fps switch
            {
                FpsMode.FPS_30 => "30 FPS",
                FpsMode.FPS_60 => "60 FPS",
                FpsMode.FPS_120 => "120 FPS",
                FpsMode.FPS_144 => "144 FPS",
                FpsMode.FPS_240 => "240 FPS",
                FpsMode.FPS_unlimited => "Unlimited",
                _ => "Unknown"
            };
        }
    }
}