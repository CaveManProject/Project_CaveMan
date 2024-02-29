using Godot;

namespace Caveman.Enums
{
    public enum Resolution
    {
        R_800x600,
        R_1024x768,
        R_1280x720,
        R_1366x768,
        R_1600x900,
        R_1920x1080
    }

    public static class ResolutionExtensions
    {
        public static string ToDisplayString(this Resolution resolution)
        {
            return resolution switch
            {
                Resolution.R_800x600 => "800x600",
                Resolution.R_1024x768 => "1024x768",
                Resolution.R_1280x720 => "1280x720",
                Resolution.R_1366x768 => "1366x768",
                Resolution.R_1600x900 => "1600x900",
                Resolution.R_1920x1080 => "1920x1080",
                _ => "Unknown"
            };
        }

        public static Vector2I ToSize(this Resolution resolution)
        {
            return resolution switch
            {
                Resolution.R_800x600 => new Vector2I(800, 600),
                Resolution.R_1024x768 => new Vector2I(1024, 768),
                Resolution.R_1280x720 => new Vector2I(1280, 720),
                Resolution.R_1366x768 => new Vector2I(1366, 768),
                Resolution.R_1600x900 => new Vector2I(1600, 900),
                Resolution.R_1920x1080 => new Vector2I(1920, 1080),
                _ => new Vector2I(0, 0)
            };
        }
    }
}