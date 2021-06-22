using NoSnoozeNET.PluginSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NoSnoozeNET.PluginSystem
{
    public class PluginLoader
    {
        public static List<ISnoozePlugin> Plugins { get; set; }

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
                //Create a new instance of all found types
                Plugins.Add((ISnoozePlugin)Activator.CreateInstance(type));
            }
        }
    }
}