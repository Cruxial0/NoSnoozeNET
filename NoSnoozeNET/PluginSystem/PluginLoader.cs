using NoSnoozeNET.PluginSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.Extensions.WPF;

namespace NoSnoozeNET.PluginSystem
{
    public class PluginLoader
    {
        public static List<ISnoozePlugin> Plugins { get; set; }
        public static List<Plugin> PluginObjects = new List<Plugin>();

        public void LoadPlugins()
        {
            Plugins = new List<ISnoozePlugin>();

            //Load the DLLs from the Plugins directory
            if (Directory.Exists(Constants.FolderName))
            {
                string[] files = Directory.GetFiles(Constants.FolderName);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }
            }

            Type interfaceType = typeof(ISnoozePlugin);
            //Fetch all types that implement the interface IPlugin and are a class
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            foreach (Type type in types)
            {
                var plugin = (ISnoozePlugin) Activator.CreateInstance(type);

                var pluginItem = new Plugin()
                {
                    //ImageIcon = ImageExt.ByteArrayToImage(plugin.Icon.IconBytes),
                    PluginInfo = new SnoozePluginInfo()
                    {
                        PluginDescription = plugin.Description,
                        PluginName = plugin.Name,
                        PluginIconInfo = plugin.Icon,
                        PluginConfig = new PluginConfig()
                    },
                };

                if (!pluginItem.PluginInfo.ConfigExists())
                {
                    plugin.CommitConfig();
                    pluginItem.PluginInfo.PluginConfig = plugin.Config;
                }
                else pluginItem.PluginInfo.LoadConfig();

                PluginObjects.Add(pluginItem);

                //Create a new instance of all found types
                Plugins.Add(plugin);
            }

        }
    }
}