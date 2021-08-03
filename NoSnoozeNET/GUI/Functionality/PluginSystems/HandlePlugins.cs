using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSnoozeNET.PluginSystem;
using NoSnoozeNET.PluginSystem.Interfaces;

namespace NoSnoozeNET.GUI.Functionality.PluginSystems
{
    public class HandlePlugins
    {
        public bool CheckConditions(IEnumerable<Plugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                ISnoozePlugin snoozePlugin = PluginLoader.Plugins.Find(x => x.Name == plugin.PluginInfo.PluginName);

                if (!snoozePlugin.Execute(new string[0])) return false;
            }

            return true;
        }
    }
}
