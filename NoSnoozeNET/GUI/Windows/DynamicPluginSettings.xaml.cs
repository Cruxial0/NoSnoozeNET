using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Namotion.Reflection;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Functionality.PluginSettings;
using NoSnoozeNET.PluginSystem;
using Xceed.Wpf.Toolkit;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for DynamicPluginSettings.xaml
    /// </summary>
    public partial class DynamicPluginSettings : Window
    {
        private Dictionary<object, UIElement> pluginDetails = new Dictionary<object, UIElement>();

        public DynamicPluginSettings(Plugin plugin)
        {
            InitializeComponent();

            GenerateControls(plugin);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, TopBar);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, btnSave);
        }

        private void GenerateControls(Plugin plugin)
        {
            if (plugin.PluginInfo.PluginConfig.BoolConfig.Count == 0 &&
                plugin.PluginInfo.PluginConfig.DateTimeConfig.Count == 0 &&
                plugin.PluginInfo.PluginConfig.StringConfig.Count == 0 &&
                plugin.PluginInfo.PluginConfig.IntConfig.Count == 0 || plugin.PluginInfo.PluginConfig == null)
            {
                Label lbl = new Label();
                lbl.Content = "No settings available :'(";
                lbl.Foreground = MainWindow.GlobalConfig.BrushConfig.MainBrush.LabelBrush;
                lbl.Margin = new Thickness(5, 10, 5, 0);
                lbl.FontSize = 12;
                lbl.FontFamily = new FontFamily("Segoe UI Light");

                StackPanel.Children.Add(lbl);
            }

            ControlDesigner controlDesigner = new ControlDesigner();

            if (plugin.PluginInfo.PluginConfig.StringConfig.Count != 0)
                foreach (var stringConfig in plugin.PluginInfo.PluginConfig.StringConfig)
                    StackPanel.Children.Add(controlDesigner.GenerateStringConfig(stringConfig));

            if (plugin.PluginInfo.PluginConfig.IntConfig.Count != 0)
                foreach (var intConfig in plugin.PluginInfo.PluginConfig.IntConfig)
                    StackPanel.Children.Add(controlDesigner.GenerateIntConfig(intConfig));

            if (plugin.PluginInfo.PluginConfig.DateTimeConfig.Count != 0)
                foreach (var dateTimeConfig in plugin.PluginInfo.PluginConfig.DateTimeConfig)
                    StackPanel.Children.Add(controlDesigner.GenerateDateTimeConfig(dateTimeConfig));

            if (plugin.PluginInfo.PluginConfig.BoolConfig.Count != 0)
            {
                StackPanel.Children.Add(controlDesigner.GenerateBoolConfig(plugin.PluginInfo.PluginConfig.BoolConfig));
            }
        }

        private void TopBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
