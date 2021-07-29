using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.PluginSystem.Interfaces;
using Image = System.Drawing.Image;

namespace NoSnoozeNET.PluginSystem.Plugins
{
    class TestPlugin2 : ISnoozePlugin
    {
        public string Name => "Test Plugin 2";
        public string Description => "Test plugin for NoSnoozeNET.";

        public PluginConfig Config { get; set; }

        public IconInfo Icon => new IconInfo()
        {
            IconBytes = ImageExt.ImageToByteArray(Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\SimpleLeague.png"))),
            IconName = "TestPluginIcon"
        };
        public bool Execute(string[] parameters)
        {
            throw new NotImplementedException();
        }

        public void CommitConfig()
        {
            this.Config = new PluginConfig();
            PluginConfig config = new PluginConfig()
            {
                StringConfig = new Dictionary<string, string>()
            };
            config.StringConfig.Add("Some path", string.Empty);
        }
    }
}
