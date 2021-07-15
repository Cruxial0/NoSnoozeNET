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
            throw new NotImplementedException();
        }
    }
}
