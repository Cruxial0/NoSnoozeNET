using NoSnoozeNET.PluginSystem.Interfaces;
using System;

namespace NoSnoozeNET.PluginSystem.Utils
{
    public class Logger
    {
        public static void Log(string msg, ConsoleColor color = ConsoleColor.Gray)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        public static void LogPlugin(ISnoozePlugin plugin)
        {
            Log($"PLUGIN REGISTERED: {plugin.Name}", ConsoleColor.Yellow);
        }

        public static void LogWarning(string msg)
        {
            Log($"WARNING: {msg}", ConsoleColor.Yellow);
        }

        public static void LogError(string msg)
        {
            Log($"ERROR: {msg}", ConsoleColor.Red);
        }

        public static void LogDebug(string msg)
        {
            Log($"DEBUG: {msg}", ConsoleColor.Blue);
        }
    }
}