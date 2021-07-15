using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.PluginSystem.Interfaces;

namespace NoSnoozeNET.PluginSystem.Plugins
{
    class TestPlugin2 : ISnoozePlugin
    {
        public string Name => "Test Plugin 2";
        public string Description => "Test plugin for NoSnoozeNET.";

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
            throw new NotImplementedException();
        }
    }
}
