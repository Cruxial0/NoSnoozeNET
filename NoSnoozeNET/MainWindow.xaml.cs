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

            //AlarmItemCollection = new ObservableCollection<AlarmItem>().Load();

            AlarmListElement = AlarmListBorder;

            TopBarElement = dckTopBar;

            _alarmItemList = new ObservableCollection<AlarmItem>().Load();

            //#region AlarmSpam

            //AlarmItem a = new AlarmItem()
            //{
            //    AlarmName = "Alarm name",
            //    AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    TimeToRing = "Rings in 7h",
            //    RingsAt = DateTime.Now.AddHours(7)
            //};

            //AlarmItem a2 = new AlarmItem()
            //{
            //    AlarmName = "Alarm nameer",
            //    AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    TimeToRing = "Rings in 9h",
            //    RingsAt = DateTime.Now.AddHours(9)

            //};

            //AlarmItem a3 = new AlarmItem()
            //{
            //    AlarmName = "Alarm namerino",
            //    AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    TimeToRing = "Rings in 3h",
            //    RingsAt = DateTime.Now.AddHours(3)

            //};

            //AlarmItem a4 = new AlarmItem()
            //{
            //    AlarmName = "Alarm namerinoas",
            //    AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    TimeToRing = "Rings in 3h",
            //    RingsAt = DateTime.Now.AddHours(3)

            //};

            //CreateAlarmItem ca = new();

            //AlarmItemCollection.Add(a);
            //AlarmItemCollection.Add(a2);
            //AlarmItemCollection.Add(a3);
            //AlarmItemCollection.Add(a4);

            //#endregion

            //foreach (var alarm in _alarmItemList)
            //{
            //    AlarmItemCollection.Add(alarm);
            //}

            AlarmList.ItemsSource = AlarmItemCollection;

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
                    //ca.SavedItem.InitializePlugins();
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
    }
}