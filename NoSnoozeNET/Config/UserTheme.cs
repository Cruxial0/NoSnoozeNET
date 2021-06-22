using System.IO;

namespace NoSnoozeNET.Config
{
    public class UserTheme
    {
        public static readonly string ConfigPath = Path.Combine(MainWindow.BinDirectory, @"Config\GlobalConfig\ThemePresets.json");

        public string ThemeName { get; set; }
        public BrushConfig brushConfig { get; set; }

        public UserTheme()
        {
            this.brushConfig = new BrushConfig();
        }
    }
}
