using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NoSnoozeNET.Extensions.Imaging;
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
        public string PluginName { get; set; }
        public string PluginDescription { get; set; }
        public IconInfo PluginIconInfo { get; set; }
        public PluginConfig PluginConfig { get; set; }

        public SnoozePluginInfo()
        {
            this.PluginIconInfo = new IconInfo();
            this.PluginConfig = new PluginConfig();
        }
    }
}
