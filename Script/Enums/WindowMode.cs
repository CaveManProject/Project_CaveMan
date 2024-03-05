namespace Caveman.Enums
{
    public enum WindowMode
    {
        FullScreen,
        Windowed,
        Borderless
    }

    public static class WindowModeExtensions
    {
        public static string ToDisplayString(this WindowMode windowMode)
        {
            return windowMode switch
            {
                WindowMode.FullScreen => "Full Screen",
                WindowMode.Windowed => "Windowed",
                WindowMode.Borderless => "Borderless",
                _ => "Unknown"
            };
        }
    }
}