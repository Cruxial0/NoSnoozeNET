using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.GUI.Functionality.Theme;
using NoSnoozeNET.GUI.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using NoSnoozeNET.PluginSystem;

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

            AlarmItemCollection = new ObservableCollection<AlarmItem>();

            Startup();
        }

        void Startup()
        {
            GlobalConfig.BrushConfig = new BrushConfig().LoadConfig();
            GlobalConfig.SelectedTheme = new ThemeHandler().LoadSelectedTheme();

            AlarmItemCollection.CollectionChanged += AlarmItemCollection_PropertyChanged;
            _alarmItemList = new ObservableCollection<AlarmItem>().Load();
            AlarmList.ItemsSource = AlarmItemCollection;

            AlarmListElement = AlarmListBorder;
            TopBarElement = dckTopBar;

            PluginLoader pluginLoader = new PluginLoader();
            pluginLoader.LoadPlugins();

            foreach (var alarmItem in _alarmItemList)
            {
                alarmItem.InitializePlugins();
            }

            if (AlarmItemCollection.Count == 0)
            {
                var createAlarm = new List<CreateAlarmItem>();

                CreateAlarmItem createAlarmItem = new();
                createAlarm.Add(createAlarmItem);

                AlarmList.ItemsSource = createAlarm;
            }

            AlarmItemCollection.Save();

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.AlarmListBorder);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.dckTopBar);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            StyleSettings ss = new();
            ss.Show();
        }

        private void CreateAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            CreateAlarm ca = new CreateAlarm();
            if (ca.ShowDialog() == true)
            {
                if (ca.SavedItem != null)
                {
                    ca.SavedItem.InitializePlugins();
                    AlarmItemCollection.Add(ca.SavedItem);
                    AlarmItemCollection.Save();
                    WindowExt.Refresh(ca.SavedItem);
                }
            }
        }

        public ObservableCollection<AlarmItem> AlarmItemCollection
        {
            get => _alarmItemList;
            set { _alarmItemList = value; NotifyPropertyChanged(nameof(AlarmItemCollection)); }
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

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void BtnPower_OnClick(object sender, RoutedEventArgs e)
        {
            GlobalConfig.BrushConfig.BindConfig();
            GlobalConfig.BrushConfig.SaveConfig();
            AlarmItemCollection.Save();
            Application.Current.Shutdown();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            StyleSettings ss = new StyleSettings();
            ss.Show();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateAlarm ca = new CreateAlarm();
            if (ca.ShowDialog() == true)
            {
                if (ca.SavedItem != null)
                {
                    ca.SavedItem.InitializePlugins();
                    AlarmItemCollection.Add(ca.SavedItem);
                    AlarmItemCollection.Save();
                }
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnPlugin_OnClick(object sender, RoutedEventArgs e)
        {
            PluginSettings PS = new PluginSettings();
            PS.Show();
        }
    }
}