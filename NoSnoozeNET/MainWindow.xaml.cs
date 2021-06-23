using NoSnoozeNET.Config;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.GUI.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Effects;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.IO;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Functionality.Theme;

namespace NoSnoozeNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string BinDirectory = Environment.CurrentDirectory;
        public static readonly string StartupDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
        public static ICollection<AlarmItem> alarmList;
        public static List<UserTheme> Themes = new List<UserTheme>();
        public static GlobalConfig GlobalConfig = new();

        public MainWindow()
        {
            InitializeComponent();

            Startup();
        }

        void Startup()
        {
            GlobalConfig.BrushConfig = new BrushConfig().LoadConfig();

            alarmList = new List<AlarmItem>();

            #region AlarmSpam

            AlarmItem a = new AlarmItem()
            {
                AlarmName = "Alarm name",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings in 7h",
            };

            AlarmItem a2 = new AlarmItem()
            {
                AlarmName = "Alarm nameer",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings in 9h",

            };

            AlarmItem a3 = new AlarmItem()
            {
                AlarmName = "Alarm namerino",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings in 3h",

            };

            AlarmItem a4 = new AlarmItem()
            {
                AlarmName = "Alarm namerinoas",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings in 3h",

            };

            CreateAlarmItem ca = new();

            alarmList.Add(a);
            alarmList.Add(a2);
            alarmList.Add(a3);
            alarmList.Add(a4);

            #endregion

            AlarmList.ItemsSource = alarmList;

            if (alarmList.Count == 0)
            {
                var createAlarm = new List<CreateAlarmItem>();

                CreateAlarmItem createAlarmItem = new();
                createAlarm.Add(createAlarmItem);

                AlarmList.ItemsSource = createAlarm;
            }

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.AlarmList);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            StyleSettings ss = new();
            ss.Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            GlobalConfig.BrushConfig.BindConfig();
            GlobalConfig.BrushConfig.SaveConfig();
        }
    }
}