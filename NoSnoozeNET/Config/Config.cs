using System;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using NoSnoozeNET.Extensions.IO;
using NoSnoozeNET.GUI.Functionality.Theme;
using NoSnoozeNET.GUI.Windows;

namespace NoSnoozeNET.Config
{
    /// <summary>
    /// Collection of Configs used throughout the project.
    /// </summary>
    public class GlobalConfig
    {
        public BrushConfig BrushConfig { get; set; }
        public Dictionary<string, BrushConfig> ThemeConfig { get; set; }
        public string SelectedTheme { get; set; }

        public GlobalConfig()
        {
            //Un-nullify children
            this.BrushConfig = new BrushConfig();
            this.ThemeConfig = new Dictionary<string, BrushConfig>();

            //Build ThemeConfig
            BuildThemeConfig();
        }

        public void BuildThemeConfig()
        {
            //Creates Config directories if they don't already exist.
            DirectoryExt.CreateFolderIfNotExist(Path.Combine(MainWindow.BinDirectory, @"\Config"));
            DirectoryExt.CreateFolderIfNotExist(BrushConfig.ConfigDirectory);
            DirectoryExt.CreateFolderIfNotExist(ThemeHandler.ThemeDirectory);

            //Declare new ThemeConfig.
            this.ThemeConfig = new Dictionary<string, BrushConfig>();

            //Create instance of ThemeHandler.
            ThemeHandler themeHandler = new ThemeHandler();

            //Add all preset themes.
            foreach (var theme in ThemePresetMethods.LoadPresetThemes())
            {
                this.ThemeConfig.Add(theme.ThemeName, theme.brushConfig);
            }

            //Add all external themes.
            foreach (var theme in themeHandler.LoadThemes())
            {
                this.ThemeConfig.Add(theme.ThemeName, theme.brushConfig);
            }
        }
    }

    public static class BrushConfigMethods
    {
        /// <summary>
        /// Applies BrushConfig to Application Resources.
        /// </summary>
        /// <param name="brushConfig">BrushConfig to apply from.</param>
        public static void ApplyBrushConfig(this BrushConfig brushConfig)
        {
            //MainColors.xaml
            Application.Current.Resources["LabelBrush"] = brushConfig.MainBrush.LabelBrush;
            Application.Current.Resources["BorderBrush"] = brushConfig.MainBrush.ControlBorderBrush;
            Application.Current.Resources["BackgroundBrush"] = brushConfig.MainBrush.BackgroundColorBrush;
            Application.Current.Resources["ControlBackgroundBrush"] = brushConfig.MainBrush.ControlBackgroundBrush;
            Application.Current.Resources["ShadowColor"] = brushConfig.MainBrush.ShadowColorBrush;

            //AlarmItem.xaml (UserControlColors.xaml)
            Application.Current.Resources["StopwatchColor"] = brushConfig.AlarmItemBrush.StopwatchBrush;
            Application.Current.Resources["OptionsColor"] = brushConfig.AlarmItemBrush.OptionsBrush;
            Application.Current.Resources["DescriptionBrush"] = brushConfig.AlarmItemBrush.DescriptionBrush;
            Application.Current.Resources["HeaderBrush"] = brushConfig.AlarmItemBrush.HeaderBrush;


            //Perform garbage collection.
            GC.Collect();
        }

        public static BrushConfig LoadConfig(this BrushConfig brushConfig)
        {
            //Check if BrushConfig Exists.
            if (File.Exists(BrushConfig.ConfigPath))
            {
                //Read BrushConfig from json.
                brushConfig = JsonConvert.DeserializeObject<BrushConfig>(File.ReadAllText(BrushConfig.ConfigPath));
            }
            else
            {
                //If not exist, set BrushConfig to Application Defaults.
                brushConfig.BindConfig();
            }

            //Apply the BrushConfig.
            brushConfig.ApplyBrushConfig();

            return brushConfig;
        }

        /// <summary>
        /// Sets BrushConfig to Application Defaults.
        /// </summary>
        /// <param name="brushConfig">BrushConfig to Bind.</param>
        /// <returns></returns>
        public static BrushConfig BindConfig(this BrushConfig brushConfig)
        {
            //MainColors.xaml
            brushConfig.MainBrush.LabelBrush = (SolidColorBrush)Application.Current.Resources["LabelBrush"];
            brushConfig.MainBrush.ControlBorderBrush = (SolidColorBrush)Application.Current.Resources["BorderBrush"];
            brushConfig.MainBrush.BackgroundColorBrush = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            brushConfig.MainBrush.ControlBackgroundBrush = (SolidColorBrush)Application.Current.Resources["ControlBackgroundBrush"];
            brushConfig.MainBrush.ShadowColorBrush = (SolidColorBrush)Application.Current.Resources["ShadowColor"];

            //AlarmItem.xaml (UserControlColors.xaml)
            brushConfig.AlarmItemBrush.StopwatchBrush = (SolidColorBrush)Application.Current.Resources["StopwatchColor"];
            brushConfig.AlarmItemBrush.OptionsBrush = (SolidColorBrush)Application.Current.Resources["OptionsColor"];
            brushConfig.AlarmItemBrush.DescriptionBrush = (SolidColorBrush)Application.Current.Resources["DescriptionBrush"];
            brushConfig.AlarmItemBrush.HeaderBrush = (SolidColorBrush)Application.Current.Resources["HeaderBrush"];

            //Perform Garbage Collection.
            GC.Collect();

            //Return BrushConfig with Application Defaults.
            return brushConfig;
        }

        public static void ColorAlarmList(List<AlarmItem> alarmItems)
        {
            //Color all AlarmItems in a list.
            foreach (var alarm in alarmItems)
            {
                alarm.ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush);
                alarm.ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush);
            }
        }
    }

    public static class ThemePresetMethods
    {
        /// <summary>
        /// Loads all pre-defined Themes.
        /// </summary>
        /// <returns>List of UserTheme.</returns>
        public static List<UserTheme> LoadPresetThemes()
        {
            //Declare List of UserTheme
            List<UserTheme> presets = new List<UserTheme>();

            //Check if UserTheme Config exists.
            if (File.Exists(UserTheme.ConfigPath))
            {
                //Read Config from json.
                presets = JsonConvert.DeserializeObject<List<UserTheme>>(File.ReadAllText(BrushConfig.ConfigPath));
            }
            else
            {
                //Generate Default Presets.
                presets = GenerateDefaultPresets(presets);
            }

            //Return themes.
            return presets;
        }

        public static void ApplyTheme(BrushConfig config)
        {
            //Modify Config
            MainWindow.GlobalConfig.BrushConfig = config;
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            //Refresh MainWindow.xaml
            WindowExt.Refresh(Application.Current.MainWindow);

            //Color all AlarmItems accordingly.
            BrushConfigMethods.ColorAlarmList(MainWindow.alarmList.ToList());
            BrushConfigMethods.ColorAlarmList(StyleSettings.PreviewItems);

            //Perform full Garbage Collection.
            GC.WaitForFullGCApproach();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        internal static List<UserTheme> GenerateDefaultPresets(List<UserTheme> presets)
        {
            presets = new List<UserTheme>();

            #region Dark Pastel by Karico

            UserTheme baseTheme = new()
            {
                ThemeName = "Dark Pastel by Karico",
                brushConfig = new()
                {
                    MainBrush = new MainBrush
                    {
                        UsingAdvanced = false,
                        LabelBrush = new SolidColorBrush(Colors.White),
                        ControlBorderBrush = new SolidColorBrush(Colors.Transparent),
                        BackgroundColorBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#3f3f4d")),
                        ControlBackgroundBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#2f2f3d"))
                    },
                    AlarmItemBrush = new AlarmItemBrush()
                    {
                        StopwatchBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#f59853")),
                        OptionsBrush = new SolidColorBrush(Colors.White),
                        DescriptionBrush = new SolidColorBrush(Colors.White),
                        HeaderBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#4f79ff")),
                    },
                    ShadowConfig = new ShadowConfig()
                    {
                        BlurRadius = 8.28071956597608,
                        Direction = 0.0,
                        ShadowDepth = 0.0,
                        Opacity = 4.0927694406548429
                    }
                }
            };

            presets.Add(baseTheme);

            #endregion

            #region Test Theme by Cruxial

            UserTheme testTheme = new()
            {
                ThemeName = "Test Theme by Cruxial",
                brushConfig = new()
                {
                    MainBrush = new MainBrush
                    {
                        UsingAdvanced = false,
                        LabelBrush = new SolidColorBrush(Colors.White),
                        ControlBorderBrush = new SolidColorBrush(Colors.Transparent),
                        BackgroundColorBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#121212")),
                        ControlBackgroundBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#1e1e1e"))
                    },
                    AlarmItemBrush = new AlarmItemBrush()
                    {
                        StopwatchBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#03DAC6")),
                        OptionsBrush = new SolidColorBrush(Colors.White),
                        DescriptionBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFFFFF")),
                        HeaderBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#BB86FC"))
                    },
                    ShadowConfig = new ShadowConfig()
                    {
                        BlurRadius = 8.28071956597608,
                        Direction = 0.0,
                        ShadowDepth = 0.0,
                        Opacity = 4.0927694406548429
                    }
                }

            };

            presets.Add(testTheme);

            #endregion

            return presets;
        }
    }
}
