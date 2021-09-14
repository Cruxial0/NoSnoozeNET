using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.PluginSystem.Interfaces;
using Image = System.Drawing.Image;

namespace NoSnoozeNET.PluginSystem.Plugins
{
    public class TestPlugin : ISnoozePlugin
    {
        public string Name => "Test Plugin";
        public string Description => "Test plugin for NoSnoozeNET.";

        public PluginConfig Config { get; set; }

        public IconInfo Icon => new IconInfo()
        {
            IconBytes = ImageExt.ImageToByteArray(Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\youtube-logo-png-picture-13.png"))),
            IconName = "TestPluginIcon"
        };

        public Grid RingSummary { get; set; }

        public bool Execute(string[] parameters)
        {
            return true;
        }

        public void CommitConfig()
        {
            this.Config = new PluginConfig()
            {
                StringConfig = new Dictionary<string, string>(),
                DateTimeConfig = new Dictionary<string, DateTime>(),
                BoolConfig = new Dictionary<string, bool>(),
            };

            Config.StringConfig.Add("Test settings", string.Empty);
            Config.StringConfig.Add("Test settings 2", string.Empty);
            Config.StringConfig.Add("Test settings 3434", string.Empty);

            Config.DateTimeConfig.Add("DateTime", new DateTime());

            Config.BoolConfig.Add("Use setting 1", new bool());
            Config.BoolConfig.Add("Use setting 2", new bool());
            Config.BoolConfig.Add("Use setting 3", new bool());

            Config.IntConfig.Add("Int setting", new int());
        }

        public void SetGridProperties()
        {
            RingSummary = new Grid()
            {
                Name = this.Name.Replace(" ", "_"),
                ColumnDefinitions =
                {
                    new ColumnDefinition{Width = GridLength.Auto},
                    new ColumnDefinition{Width = GridLength.Auto}
                },
                RowDefinitions =
                {
                    new RowDefinition{Height = GridLength.Auto},
                    new RowDefinition{Height = GridLength.Auto}
                },
                Width = Double.NaN,
                Height = Double.NaN,
                Visibility = Visibility.Visible,
            };

            Label lblProp1 = new Label
            {
                Content = "Property 1",
                Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush,
            };
            lblProp1.SetValue(Grid.RowProperty, 0);
            lblProp1.SetValue(Grid.ColumnProperty, 0);

            Label lblPropValue1 = new Label
            {
                Content = "Value 1",
                Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.DescriptionBrush,
            };
            lblPropValue1.SetValue(Grid.RowProperty, 0);
            lblPropValue1.SetValue(Grid.ColumnProperty, 1);

            Label lblProp2 = new Label
            {
                Content = "Property 2",
                Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush,
            };
            lblProp2.SetValue(Grid.RowProperty, 1);
            lblProp2.SetValue(Grid.ColumnProperty, 0);

            Label lblPropValue2 = new Label
            {
                Content = "Value 2",
                Foreground = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.DescriptionBrush,
            };
            lblPropValue2.SetValue(Grid.RowProperty, 1);
            lblPropValue2.SetValue(Grid.ColumnProperty, 1);

            RingSummary.Children.Add(lblProp1);
            RingSummary.Children.Add(lblPropValue1);
            RingSummary.Children.Add(lblProp2);
            RingSummary.Children.Add(lblPropValue2);
        }
    }
}
