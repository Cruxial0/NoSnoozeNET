using System;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NoSnoozeNET.GUI.Functionality.Theme;

namespace NoSnoozeNET.Config
{
    public class GlobalConfig
    {
        public BrushConfig BrushConfig { get; set; }
        public Dictionary<string, BrushConfig> ThemeConfig { get; set; }
        public string SelectedTheme { get; set; }

        public GlobalConfig()
        {
            this.BrushConfig = new BrushConfig();
            this.ThemeConfig = new Dictionary<string, BrushConfig>();

            BuildThemeConfig();
        }

        public void BuildThemeConfig()
        {
            this.ThemeConfig = new Dictionary<string, BrushConfig>();

            ThemeHandler themeHandler = new ThemeHandler();

            foreach (var theme in ThemePresetMethods.LoadPresetThemes())
            {
                this.ThemeConfig.Add(theme.ThemeName, theme.brushConfig);
            }

            foreach (var theme in themeHandler.LoadThemes())
            {
                this.ThemeConfig.Add(theme.ThemeName, theme.brushConfig);
            }
        }
    }

    public static class BrushConfigMethods
    {
        public static void ApplyBrushConfig(this BrushConfig brushConfig)
        {
            //MainColors.xaml
            Application.Current.Resources["LabelBrush"] = brushConfig.MainBrush.LabelBrush;
            Application.Current.Resources["SecondaryBrush"] = brushConfig.MainBrush.SecondaryColorBrush;
            Application.Current.Resources["BackgroundBrush"] = brushConfig.MainBrush.BackgroundColorBrush;
            Application.Current.Resources["ControlBackgroundBrush"] = brushConfig.MainBrush.ControlBackgroundBrush;

            //AlarmItem.xaml (UserControlColors.xaml)
            Application.Current.Resources["StopwatchColor"] = brushConfig.AlarmItemBrush.StopwatchBrush;
            Application.Current.Resources["OptionsColor"] = brushConfig.AlarmItemBrush.OptionsBrush;
            Application.Current.Resources["DescriptionBrush"] = brushConfig.AlarmItemBrush.DescriptionBrush;
            Application.Current.Resources["HeaderBrush"] = brushConfig.AlarmItemBrush.HeaderBrush;

            GC.Collect();
        }

        public static void LoadConfig(this BrushConfig brushConfig)
        {
            if (File.Exists(BrushConfig.ConfigPath))
            {
                brushConfig = JsonConvert.DeserializeObject<BrushConfig>(File.ReadAllText(BrushConfig.ConfigPath));
            }
            else
            {
                brushConfig.BindConfig();
            }

            brushConfig.ApplyBrushConfig();
        }

        public static BrushConfig BindConfig(this BrushConfig brushConfig)
        {
            //MainColors.xaml
            brushConfig.MainBrush.LabelBrush = (SolidColorBrush)Application.Current.Resources["LabelBrush"];
            brushConfig.MainBrush.SecondaryColorBrush = (SolidColorBrush)Application.Current.Resources["SecondaryBrush"];
            brushConfig.MainBrush.BackgroundColorBrush = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            brushConfig.MainBrush.ControlBackgroundBrush = (SolidColorBrush)Application.Current.Resources["ControlBackgroundBrush"];

            //AlarmItem.xaml (UserControlColors.xaml)
            brushConfig.AlarmItemBrush.StopwatchBrush = (SolidColorBrush)Application.Current.Resources["StopwatchColor"];
            brushConfig.AlarmItemBrush.OptionsBrush = (SolidColorBrush)Application.Current.Resources["OptionsColor"];
            brushConfig.AlarmItemBrush.DescriptionBrush = (SolidColorBrush)Application.Current.Resources["DescriptionBrush"];
            brushConfig.AlarmItemBrush.HeaderBrush = (SolidColorBrush)Application.Current.Resources["HeaderBrush"];

            GC.Collect();

            return brushConfig;
        }

        public static void ColorAlarmList(List<AlarmItem> alarmItems)
        {
            foreach (var alarm in alarmItems)
            {
                var item = alarmItems.First(x => alarm == x);

                item.ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush);
                item.ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush);
            }
        }
    }

    public static class ThemePresetMethods
    {
        public static List<UserTheme> LoadPresetThemes()
        {
            List<UserTheme> presets = new List<UserTheme>();

            if (File.Exists(UserTheme.ConfigPath))
            {
                presets = JsonConvert.DeserializeObject<List<UserTheme>>(File.ReadAllText(BrushConfig.ConfigPath)); ;
            }
            else
            {
                presets = GenerateDefaultPresets(presets);
            }

            return presets;
        }

        public static void ApplyTheme(BrushConfig config)
        {
            //Modify Config
            MainWindow.GlobalConfig.BrushConfig = config;
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            //Refresh MainWindow.xaml
            WindowExt.Refresh(Application.Current.MainWindow);

            //Color all AlarmItems in MainWindow.xaml2
            BrushConfigMethods.ColorAlarmList(MainWindow.alarmList.ToList());

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
                        SecondaryColorBrush = new SolidColorBrush(Colors.Transparent),
                        BackgroundColorBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#3f3f4d")),
                        ControlBackgroundBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#2f2f3d"))
                    },
                    AlarmItemBrush = new AlarmItemBrush()
                    {
                        StopwatchBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#f59853")),
                        OptionsBrush = new SolidColorBrush(Colors.White),
                        DescriptionBrush = new SolidColorBrush(Colors.White),
                        HeaderBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#4f79ff")),
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
                        SecondaryColorBrush = new SolidColorBrush(Colors.Transparent),
                        BackgroundColorBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#121212")),
                        ControlBackgroundBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#1e1e1e"))
                    },
                    AlarmItemBrush = new AlarmItemBrush()
                    {
                        StopwatchBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#03DAC6")),
                        OptionsBrush = new SolidColorBrush(Colors.White),
                        DescriptionBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFFFFF")),
                        HeaderBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#BB86FC"))
                    }
                }

            };

            presets.Add(testTheme);

            #endregion

            return presets;
        }
    }
}
