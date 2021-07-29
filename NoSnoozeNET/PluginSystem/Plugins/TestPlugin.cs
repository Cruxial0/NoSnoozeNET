using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool Execute(string[] parameters)
        {
            throw new NotImplementedException();
        }

        public void CommitConfig()
        {
            TextBox tb = new TextBox();
            tb.Width = 100;
            tb.Height = 20;
            tb.Text = "Test";

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
        }
    }


}
