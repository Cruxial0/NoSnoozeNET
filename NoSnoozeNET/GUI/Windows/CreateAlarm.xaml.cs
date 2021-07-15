using NoSnoozeNET.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NoSnoozeNET.Extensions.WPF;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for CreateAlarm.xaml
    /// </summary>
    public partial class CreateAlarm : Window
    {
        public static List<AlarmItem> PreviewItemList = new List<AlarmItem>();
        public AlarmItem PreviewItem = new();
        public AlarmItem OutputAlarmItem;

        private List<PluginListItem> _pluginListItems = new List<PluginListItem>();
        private List<Image> _pluginImages = new List<Image>();

        public CreateAlarm()
        {
            InitializeComponent();

            var alarmList = new List<AlarmItem>();

            PreviewItem = new AlarmItem()
            {
                AlarmName = "Alarm name",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings in 7h",
            };

            PreviewItem.LayoutTransform = new ScaleTransform(0.75, 0.75);

            alarmList.Add(PreviewItem);

            PreviewContainer.ItemsSource = alarmList;

            List<PluginListItem> plugins = new List<PluginListItem>();

            plugins.Add(new PluginListItem());
            plugins.Add(new PluginListItem());
            plugins.Add(new PluginListItem());
            plugins.Add(new PluginListItem());

            PluginList.ItemsSource = plugins;

            PreviewItemList.Add(PreviewItem);

            foreach (var plugin in PluginList.Items)
            {
                var pluginItem = plugin as PluginListItem;

                Image img = new Image();
                img.Source = pluginItem.Image.Source;

                _pluginListItems.Add(pluginItem);
                _pluginImages.Add(img);
            }

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.TopBar);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.btnSave);
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PreviewItem.AlarmName = txtAlarmName.Text;
        }

        private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var now = DateTime.Now;
            double totalHours = 0;
            if (now > TimePicker.Value.Value)
            {
                var tomorrow = now.AddDays(1).Date.AddHours(int.Parse(TimePicker.Value.Value.ToString("hh")));
                totalHours = (tomorrow - now).TotalHours;
            }
            else
            {
                totalHours = (TimePicker.Value.Value - now).TotalHours;
            }

            PreviewItem.RingsAt = TimePicker.Value.Value;
            PreviewItem.TimeToRing = $"Rings at {TimePicker.Value.Value:HH:mm}";
            PreviewItem.AlarmCreated = $"Created: {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            OutputAlarmItem = new AlarmItem()
            {
                AlarmCreated = PreviewItem.AlarmCreated,
                AlarmName = PreviewItem.AlarmName,
                TimeToRing = PreviewItem.TimeToRing,
                RingsAt = PreviewItem.RingsAt,
                PluginElements = PreviewItem.PluginElements
            };
            this.DialogResult = true;
            this.Close();
        }

        public AlarmItem SavedItem => OutputAlarmItem;

        private void TopBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PluginList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = PluginList.SelectedIndex;

            bool toggle = _pluginListItems.ElementAt(item).Toggle();

            if(!toggle) PreviewItem.RemovePlugin(_pluginImages.ElementAt(item));
            if(toggle) PreviewItem.AddPlugin(_pluginImages.ElementAt(item));
        }
    }
}