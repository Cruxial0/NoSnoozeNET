using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NoSnoozeNET.GUI.Windows;
using Xceed.Wpf.Toolkit;

namespace NoSnoozeNET.GUI.Functionality.PluginSettings
{
    public class ControlDesigner
    {
        public StackPanel GenerateStringConfig(KeyValuePair<string, string> pair)
        {
            StackPanel innerPanel = new StackPanel();
            innerPanel.Name = pair.Key.Replace(" ", "");
            innerPanel.Background = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush;
            innerPanel.Orientation = Orientation.Horizontal;
            innerPanel.FlowDirection = FlowDirection.LeftToRight;
            innerPanel.Margin = new Thickness(5);
            innerPanel.Width = Double.NaN;

            Label lbl = new Label();
            lbl.Content = pair.Key + ":";
            lbl.Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush;
            lbl.FontSize = 14;
            lbl.FontFamily = new FontFamily("Segoe UI");

            TextBox txt = new TextBox();

            txt.Text = pair.Value;
            txt.Background = null;
            txt.BorderBrush = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBorderBrush;
            txt.Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.DescriptionBrush;
            txt.FontFamily = new FontFamily("Segoe UI Light");
            txt.VerticalContentAlignment = VerticalAlignment.Center;
            txt.MinWidth = 200;

            innerPanel.Children.Add(lbl);
            innerPanel.Children.Add(txt);

            return innerPanel;
        }

        public StackPanel GenerateIntConfig(KeyValuePair<string, int> pair)
        {
            StackPanel innerPanel = new StackPanel();
            innerPanel.Name = pair.Key.Replace(" ", "");
            innerPanel.Background = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush;
            innerPanel.Orientation = Orientation.Horizontal;
            innerPanel.FlowDirection = FlowDirection.LeftToRight;
            innerPanel.Margin = new Thickness(5);
            innerPanel.Width = Double.NaN;

            Label lbl = new Label();
            lbl.Content = pair.Key + ":";
            lbl.Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush;
            lbl.FontSize = 14;
            lbl.FontFamily = new FontFamily("Segoe UI");

            TextBox txt = new TextBox();

            txt.Text = pair.Value.ToString();
            txt.Background = null;
            txt.BorderBrush = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBorderBrush;
            txt.Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.DescriptionBrush;
            txt.FontFamily = new FontFamily("Segoe UI Light");
            txt.VerticalContentAlignment = VerticalAlignment.Center;
            txt.MinWidth = 200;

            innerPanel.Children.Add(lbl);
            innerPanel.Children.Add(txt);

            return innerPanel;
        }

        public StackPanel GenerateDateTimeConfig(KeyValuePair<string, DateTime> pair)
        {
            StackPanel innerPanel = new StackPanel();
            innerPanel.Name = pair.Key.Replace(" ", "");
            innerPanel.Background = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush;
            innerPanel.Orientation = Orientation.Horizontal;
            innerPanel.FlowDirection = FlowDirection.LeftToRight;
            innerPanel.Margin = new Thickness(5);
            innerPanel.MinWidth = 200;
            innerPanel.Width = Double.NaN;

            Label lbl = new Label();
            lbl.Content = pair.Key + ":";
            lbl.Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush;
            lbl.FontSize = 14;
            lbl.FontFamily = new FontFamily("Segoe UI");

            TimePicker dtp = new TimePicker();

            dtp.Foreground = MainWindow.GlobalConfig.BrushConfig.MainBrush.LabelBrush;
            dtp.BorderBrush = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBorderBrush;
            dtp.Background = null;
            dtp.FontFamily = new FontFamily("Segoe UI Light");
            dtp.HorizontalAlignment = HorizontalAlignment.Right;
            dtp.VerticalContentAlignment = VerticalAlignment.Center;

            innerPanel.Children.Add(lbl);
            innerPanel.Children.Add(dtp);

            return innerPanel;
        }

        public StackPanel GenerateBoolConfig(Dictionary<string, bool> boolConfig)
        {
            StackPanel innerPanel = new StackPanel();
            innerPanel.Name = "BoolConfig";
            innerPanel.Background = MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush;
            innerPanel.Orientation = Orientation.Vertical;
            innerPanel.FlowDirection = FlowDirection.LeftToRight;
            innerPanel.Margin = new Thickness(5, 10, 5, 10);
            innerPanel.MinWidth = 200;
            innerPanel.Width = Double.NaN;

            foreach (var pair in boolConfig)
            {
                CheckBox chk = new CheckBox();
                chk.Name = pair.Key.Replace(" ", "");
                chk.Margin = new Thickness(5);
                chk.Content = pair.Key;
                chk.IsChecked = pair.Value;

                chk.Foreground = MainWindow.GlobalConfig.BrushConfig.MainBrush.LabelBrush;
                chk.FontFamily = new FontFamily("Segoe UI Light");
                chk.FontSize = 14;

                innerPanel.Children.Add(chk);
            }

            return innerPanel;
        }
    }
}
