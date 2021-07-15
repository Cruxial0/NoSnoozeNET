using NoSnoozeNET.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.PluginSystem;
using Image = System.Windows.Controls.Image;

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
        private List<Plugin> _plugins = new List<Plugin>();

        private Dictionary<Plugin, Image> pluginImages = new Dictionary<Plugin, Image>();

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


            foreach (var plugin in PluginLoader.PluginObjects)
            {
                //plugin.ImageIcon = ImageExt.ByteArrayToImage(plugin.PluginInfo.PluginIconInfo.IconBytes);
                var listItem = new PluginListItem()
                {
                    PluginName = plugin.PluginInfo.PluginName,
                    PluginImage = ImageExt.ByteArrayToImage(plugin.PluginInfo.PluginIconInfo.IconBytes)
                };
                Image img = new Image()
                {
                    Source = ((Bitmap) listItem.PluginImage).ToBitmapImage()
                };

                pluginImages.Add(plugin, img);

                listItem.SetImage();
                plugins.Add(listItem);
                _plugins.Add(plugin);
            }


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

        private List<UIElement> generateNewElements(List<UIElement> elements)
        {
            List<UIElement> newList = new List<UIElement>();

            foreach (var element in elements)
            {
                Image img = new Image();
                img.Source = ((Image)element).Source;

                newList.Add(img);
            }

            return newList;
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

            if(!toggle) PreviewItem.RemovePlugin(pluginImages.ElementAt(item).Value, pluginImages.ElementAt(item).Key);
            if(toggle) PreviewItem.AddPlugin(pluginImages.ElementAt(item).Value, pluginImages.ElementAt(item).Key);
        }
    }
}