using NoSnoozeNET.PluginSystem.Interfaces;
using System;

namespace NoSnoozeNET.PluginSystem.Plugins
{
    public class HelpPlugin : ISnoozePlugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IconInfo Icon { get; set; }
        bool ISnoozePlugin.Execute(string[] parameters)
        {
            //Save Old Color
            var oldColor = Console.ForegroundColor;
            Console.WriteLine(Environment.NewLine);

            //Find and write all plugins to console
            foreach (var plugin in PluginLoader.Plugins)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{plugin.Name}: ");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{plugin.Description}\n");
            }

            //Restore old color
            Console.ForegroundColor = oldColor;

            return true;
        }

        void ISnoozePlugin.CommitConfig()
        {

        }

    }
}