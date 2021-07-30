using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.Extensions.IO;
using NoSnoozeNET.PluginSystem.Interfaces;

namespace NoSnoozeNET.PluginSystem
{
    public class Plugin
    {
        public SnoozePluginInfo PluginInfo { get; set; }

        public Plugin()
        {
            this.PluginInfo = new SnoozePluginInfo();
        }
    }

    public class SnoozePluginInfo
    {
        public string PluginDirectory = Path.Combine(MainWindow.BinDirectory, @"Config\PluginConfig\");

        public string PluginName { get; set; }
        public string PluginDescription { get; set; }
        public IconInfo PluginIconInfo { get; set; }
        public PluginConfig PluginConfig { get; set; }

        public SnoozePluginInfo()
        {
            this.PluginIconInfo = new IconInfo();
            this.PluginConfig = new PluginConfig();

            DirectoryExt.CreateFolderIfNotExist(PluginDirectory);
        }

        public void SaveConfig() => File.WriteAllText(Path.Combine(this.PluginDirectory, this.PluginName + ".json"), JsonConvert.SerializeObject(this.PluginConfig, Formatting.Indented));

        public bool ConfigExists() => File.Exists(Path.Combine(this.PluginDirectory, this.PluginName + ".json"));

        public void LoadConfig()
        {
            this.PluginConfig =
                JsonConvert.DeserializeObject<PluginConfig>(
                    File.ReadAllText(Path.Combine(this.PluginDirectory, this.PluginName + ".json")));
        }
    }
}
