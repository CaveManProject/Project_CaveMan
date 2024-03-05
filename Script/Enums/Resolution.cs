using Godot;

namespace Caveman.Enums
{
    public enum Resolution
    {
        R_1152x648,
        R_640x800,
        R_1280x720,
        R_1600x900,
        R_1920x1080,
        R_2560x1440,
        R_3840x2160,
    }

    public static class ResolutionExtensions
    {
        public static string ToDisplayString(this Resolution resolution)
        {
            return resolution switch
            {
                Resolution.R_1152x648 => "1152x648",
                Resolution.R_640x800 => "640x800",
                Resolution.R_1280x720 => "1280x720",
                Resolution.R_1600x900 => "1600x900",
                Resolution.R_1920x1080 => "1920x1080",
                Resolution.R_2560x1440 => "2560x1440",
                Resolution.R_3840x2160 => "3840x2160",
                _ => "Unknown"
            };
        }

        public static Vector2I ToSize(this Resolution resolution)
        {
            return resolution switch
            {
                Resolution.R_1152x648 => new Vector2I(1152, 648),
                Resolution.R_640x800 => new Vector2I(640, 800),
                Resolution.R_1280x720 => new Vector2I(1280, 720),
                Resolution.R_1600x900 => new Vector2I(1600, 900),
                Resolution.R_1920x1080 => new Vector2I(1920, 1080),
                Resolution.R_2560x1440 => new Vector2I(2560, 1440),
                Resolution.R_3840x2160 => new Vector2I(3840, 2160),
                _ => new Vector2I(0, 0)
            };
        }
    }
}