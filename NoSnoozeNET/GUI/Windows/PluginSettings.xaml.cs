using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.PluginSystem;
using Image = System.Windows.Controls.Image;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for PluginSettings.xaml
    /// </summary>
    public partial class PluginSettings : Window
    {
        private List<PluginListItem> _pluginListItems = new List<PluginListItem>();
        private List<Image> _pluginImages = new List<Image>();
        private List<Plugin> _plugins = new List<Plugin>();

        private Dictionary<Plugin, Image> pluginImages = new Dictionary<Plugin, Image>();

        public PluginSettings()
        {
            InitializeComponent();
            LoadPlugins();

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.TopBar);
        }

        private void LoadPlugins()
        {
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

            foreach (var plugin in PluginList.Items)
            {
                var pluginItem = plugin as PluginListItem;

                Image img = new Image();
                img.Source = pluginItem.Image.Source;

                _pluginListItems.Add(pluginItem);
                _pluginImages.Add(img);
            }
        }

        private void PluginList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = PluginList.SelectedIndex;

            DynamicPluginSettings DPS = new DynamicPluginSettings(_plugins.ElementAt(item));
            DPS.Show();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TopBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
