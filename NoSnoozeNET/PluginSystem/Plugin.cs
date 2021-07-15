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
        //public System.Drawing.Image ImageIcon { get; set; }
        public SnoozePluginInfo PluginInfo { get; set; }

        public Plugin()
        {
            this.PluginInfo = new SnoozePluginInfo();
            if (PluginInfo.PluginIconInfo.IconBytes != null)
            {
                //this.ImageIcon = ImageExt.ByteArrayToImage(PluginInfo.PluginIconInfo.IconBytes);
            }
        }
    }

    public class SnoozePluginInfo
    {
        public string PluginName { get; set; }
        public string PluginDescription { get; set; }
        public IconInfo PluginIconInfo { get; set; }

        public SnoozePluginInfo()
        {
            this.PluginIconInfo = new IconInfo();
        }
    }
}
