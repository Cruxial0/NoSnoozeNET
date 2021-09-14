using System;
using System.Collections.Generic;
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
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.PluginSystem;
using NoSnoozeNET.PluginSystem.Interfaces;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for PostRingSummary.xaml
    /// </summary>
    public partial class PostRingSummary : Window
    {
        public PostRingSummary(AlarmItem alarmItem)
        {
            InitializeComponent();

            lblAlarmName.Content = alarmItem.AlarmName;
            lblPluginCount.Content = alarmItem.PluginElements.Count.ToString();

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.TopBar);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, this.MainGrid);

            LoadUI(alarmItem);
        }

        private void LoadUI(AlarmItem alarmItem)
        {
            int i = 4;
            foreach (var plugin in alarmItem.PluginElements)
            {
                ISnoozePlugin snoozePlugin = PluginLoader.Plugins.Find(x => x.Name == plugin.PluginInfo.PluginName);

                snoozePlugin.SetGridProperties();

                var summaryGrid = snoozePlugin.RingSummary;

                if (summaryGrid == null) continue;

                if(summaryGrid.Children.Count == 0) MessageBox.Show("children = 0");

                ParentGrid.RowDefinitions.Add(new RowDefinition{Height = GridLength.Auto});

                Label pluginLabel = new Label
                {
                    Content = plugin.PluginInfo.PluginName.Replace("_", " "),
                    Foreground = MainWindow.GlobalConfig.BrushConfig.MainBrush.LabelBrush,
                    FontSize = 12
                };
                pluginLabel.SetValue(Grid.RowProperty, i);
                pluginLabel.SetValue(Grid.ColumnProperty, 1);

                summaryGrid.SetValue(Grid.RowProperty, i + 1);
                summaryGrid.SetValue(Grid.ColumnProperty, 1);

                pluginLabel.SetValue(Label.MarginProperty, new Thickness(0,10,0,5));

                summaryGrid.Background = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush;

                WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, summaryGrid);

                ParentGrid.Children.Add(pluginLabel);
                ParentGrid.Children.Add(summaryGrid);

                i++;
            }
            ParentGrid.RowDefinitions.Add(new RowDefinition{Height = new GridLength(20)});
            ParentGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(20)});
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
