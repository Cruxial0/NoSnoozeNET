using System;
using Newtonsoft.Json;
using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
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

            //Write json to specified path.
            File.WriteAllText(Path.Combine(ThemeDirectory, $"{theme.ThemeName}.json"), JsonConvert.SerializeObject(theme, Formatting.Indented));

            //Add theme to GlobalConfig.
            MainWindow.GlobalConfig.AddTheme(theme);
        }

        public string LoadSelectedTheme()
        {
            if (!File.Exists(Path.Combine(BrushConfig.ConfigDirectory, "SelectedTheme.json"))) return null;
            return JsonConvert.DeserializeObject<string>(
                File.ReadAllText(Path.Combine(BrushConfig.ConfigDirectory, "SelectedTheme.json"))) ?? null;
        }

        public List<UserTheme> LoadThemes()
        {
            //Get all files with .json extension.
            string[] files = System.IO.Directory.GetFiles(ThemeDirectory, "*.json");

            //Declare new list of UserTheme.
            List<UserTheme> themes = new List<UserTheme>();
            List<string> loadingFailed = new List<string>();

            //Create json validation schema.
            var schema = NJsonSchema.JsonSchema.FromType<UserTheme>();

            //Loop through all .json files.
            foreach (var file in files)
            {
                //Validate Json with UserTheme.
                var errors = schema.Validate(File.ReadAllText(Path.Combine(ThemeDirectory, file)));

                //Check if there were no errors, if not, add theme to list.
                if(errors.Count == 0)
                    themes.Add(JsonConvert.DeserializeObject<UserTheme>(File.ReadAllText(Path.Combine(ThemeDirectory, file))));
                else
                {
                    loadingFailed.Add(file.Split('\\').Last());
                }
            }

            if(loadingFailed.Count != 0)
            {
                MessageBox.Show(
                    $"Following Themes failed to load.\nThis will not impact application performance, but consider re-creating or deleting following themes:\n{String.Join(Environment.NewLine, loadingFailed)}", "Loading Themes Failed!", MessageBoxButton.OK);
            }

            //Return list of UserTheme.
            return themes;
        }
    }
}
