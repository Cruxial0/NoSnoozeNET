using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Functionality.PluginSettings;
using NoSnoozeNET.PluginSystem;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for DynamicPluginSettings.xaml
    /// </summary>
    public partial class DynamicPluginSettings : Window
    {
        private Plugin _plugin;

        public DynamicPluginSettings(Plugin plugin)
        {
            InitializeComponent();

            GenerateControls(plugin);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, TopBar);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, btnSave);
        }

        private void GenerateControls(Plugin plugin)
        {
            _plugin = plugin;

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
                btnSave.Visibility = Visibility.Hidden;
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

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            List<string> failedList = new List<string>();

            foreach(object child in StackPanel.Children)
            {
                string childname = null;
                if (child is FrameworkElement)
                {
                    childname = (child as FrameworkElement).Name;
                }

                if (childname == null) return;

                childname = childname.Replace("_", " ");

                StackPanel childPanel = child as StackPanel;

                string type = "";

                if (_plugin.PluginInfo.PluginConfig.BoolConfig.ContainsKey(childname)) type = "bool";
                if (_plugin.PluginInfo.PluginConfig.DateTimeConfig.ContainsKey(childname)) type = "DateTime";
                if (_plugin.PluginInfo.PluginConfig.IntConfig.ContainsKey(childname)) type = "int";
                if (_plugin.PluginInfo.PluginConfig.StringConfig.ContainsKey(childname)) type = "string";

                foreach (var element in childPanel.Children)
                {
                    if(!(element is FrameworkElement)) continue;
                    if(element.GetType() == typeof(Label)) continue;

                    if (childname == "BoolConfig")
                    {
                        string boolName = (element as FrameworkElement)?.Name.Replace("_", " ");

                        if (element.GetType() != typeof(CheckBox))
                        {
                            failedList.Add(boolName);
                            continue;
                        }

                        bool? isChecked = ((CheckBox) element).IsChecked;
                        if (isChecked != null)
                            _plugin.PluginInfo.PluginConfig.BoolConfig[boolName ?? string.Empty] =
                                isChecked.Value;
                        else
                            failedList.Add(boolName);
                        continue;
                    }

                    switch (type)
                    {
                        case "string":
                            _plugin.PluginInfo.PluginConfig.StringConfig[childname] = ((TextBox) element).Text;
                            break;

                        case "int":
                            if (!int.TryParse(((TextBox) element).Text, out int n))
                            {
                                MessageBox.Show($"Invalid input in '{childname}'\nThis field can only contain numbers!");
                                break;
                            }

                            _plugin.PluginInfo.PluginConfig.IntConfig[childname] = n;
                            break;

                        case "DateTime":

                            DateTime? dateTime = ((TimePicker) element).Value;
                            if (dateTime != null)
                                _plugin.PluginInfo.PluginConfig.DateTimeConfig[childname] =
                                    dateTime.Value;
                            else
                                failedList.Add(childname);

                            break;

                        default:
                            failedList.Add(childname);
                            break;
                    }
                }
            }

            if (failedList.Count != 0)
            {
                MessageBox.Show($"Following values failed to save:\n{string.Join(Environment.NewLine, failedList)}");
                return;
            }

            _plugin.PluginInfo.SaveConfig();
        }
    }
}
