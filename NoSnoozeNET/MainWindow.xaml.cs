using NoSnoozeNET.Config;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.GUI.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static readonly string BinDirectory = Environment.CurrentDirectory;
        public static readonly string StartupDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;

        public static ObservableCollection<AlarmItem> AlarmColorList = new ObservableCollection<AlarmItem>();

        private ObservableCollection<AlarmItem> _alarmItemList;
        private AlarmItem _selectedAlarmItem;

        public static GlobalConfig GlobalConfig = new();
        public static UIElement AlarmListElement = new UIElement();
        public static UIElement TopBarElement = new UIElement();

        public MainWindow()
        {
            InitializeComponent();

            Startup();
        }

        void Startup()
        {
            GlobalConfig.BrushConfig = new BrushConfig().LoadConfig();
            GlobalConfig.SelectedTheme = new ThemeHandler().LoadSelectedTheme();


            AlarmListElement = AlarmListBorder;

            TopBarElement = dckTopBar;

            _alarmItemList = new ObservableCollection<AlarmItem>();

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

            AlarmItemCollection.Add(a);
            AlarmItemCollection.Add(a2);
            AlarmItemCollection.Add(a3);
            AlarmItemCollection.Add(a4);

            #endregion

            AlarmList.ItemsSource = AlarmItemCollection;

            if (AlarmItemCollection.Count == 0)
            {
                var createAlarm = new List<CreateAlarmItem>();

                CreateAlarmItem createAlarmItem = new();
                createAlarm.Add(createAlarmItem);

                AlarmList.ItemsSource = createAlarm;
            }

            AlarmItemCollection.CollectionChanged += AlarmItemCollection_PropertyChanged;

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.AlarmListBorder);
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

        private void CreateAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            CreateAlarm ca = new CreateAlarm();
            if (ca.ShowDialog() == true)
            {
                if (ca.SavedItem != null)
                {
                    
                }
                AlarmItemCollection.Add(ca.SavedItem);
            }
        }

        public ObservableCollection<AlarmItem> AlarmItemCollection
        {
            get => _alarmItemList;
            set => _alarmItemList = value;
        }

        public AlarmItem SelectedAlarmItem
        {
            get => _selectedAlarmItem;
            set { _selectedAlarmItem = value; NotifyPropertyChanged(nameof(SelectedAlarmItem)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            AlarmColorList = AlarmItemCollection;
        }

        private void AlarmItemCollection_PropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AlarmList.Items.Refresh();
        }
    }
}