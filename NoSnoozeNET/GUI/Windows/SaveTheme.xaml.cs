using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.IO;
using NoSnoozeNET.GUI.Functionality.Theme;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for SaveTheme.xaml
    /// </summary>
    public partial class SaveTheme : Window
    {
        internal static int unnamedIterator = 1;

        public SaveTheme()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            UserTheme ut = new UserTheme()
            {
                brushConfig = new BrushConfig().BindConfig(),
                ThemeName = DetermineUnnamedThemeName()
            };

            var path = Path.Combine(ThemeHandler.ThemeDirectory, $"{ut.ThemeName}.json");
            DirectoryExt.CreateIfNotExist(path);

            if (File.Exists(path))
            {
                if (MessageBox.Show($"A theme called '{ut.ThemeName}' already exists.\nDo you want to overwrite it?", "Duplicate Theme",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }

            ThemeHandler th = new ThemeHandler();

            th.SaveTheme(ut);

            this.Close();
        }

        private string DetermineUnnamedThemeName()
        {
            if (txtThemeName.Text != string.Empty || txtThemeName.Text != "") return txtThemeName.Text;
            if (!File.Exists(Path.Combine(ThemeHandler.ThemeDirectory, $"Unnamed{unnamedIterator}.json")))
                return $"Unnamed Theme {unnamedIterator}";

            unnamedIterator++;

            return DetermineUnnamedThemeName();
        }
    }
}
