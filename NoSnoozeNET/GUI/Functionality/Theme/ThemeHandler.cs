using Newtonsoft.Json;
using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.IO;
using System.Collections.Generic;
using System.IO;
using NoSnoozeNET.Extensions.Internal;

namespace NoSnoozeNET.GUI.Functionality.Theme
{
    public class ThemeHandler
    {
        public static readonly string ThemeDirectory = Path.Combine(MainWindow.BinDirectory, @"Config\Themes");

        public void SaveTheme(UserTheme theme)
        {
            //Check if directory exists. Create new directory if it doesn't.
            DirectoryExt.CreateIfNotExist(ThemeDirectory);

            File.WriteAllText(Path.Combine(ThemeDirectory, $"{theme.ThemeName}.json"), JsonConvert.SerializeObject(theme, Formatting.Indented));

            MainWindow.GlobalConfig.AddTheme(theme);
        }

        public List<UserTheme> LoadThemes()
        {
            string[] files = System.IO.Directory.GetFiles(ThemeDirectory, "*.json");

            List<UserTheme> themes = new List<UserTheme>();

            foreach (var file in files)
            {
                themes.Add(JsonConvert.DeserializeObject<UserTheme>(File.ReadAllText(Path.Combine(ThemeDirectory, file))));
            }

            return themes;
        }
    }
}
