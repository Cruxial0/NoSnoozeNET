using System.Collections.Generic;

namespace NoSnoozeNET.PluginSystem.Interfaces
{
    public interface ISnoozePlugin
    {
        /// <summary>
        /// Plugin Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin Description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Config surrounding the plugin.
        /// </summary>
        PluginConfig Config { get; set; }

        /// <summary>
        /// Info for associated icon.
        /// </summary>
        IconInfo Icon { get; }

        /// <summary>
        /// Execution function for plugins.
        /// </summary>
        /// <param name="parameters">Parameters used in plugin execution.</param>
        /// <returns>Boolean</returns>
        bool Execute(string[] parameters);

        /// <summary>
        /// Function for commiting local plugin settings into the system.
        /// </summary>
        void CommitConfig();

    }

    /// <summary>
    /// Icon Info for associated icon.
    /// </summary>
    public class IconInfo
    {
        /// <summary>
        /// Byte array for associated icon
        /// </summary>
        public byte[] IconBytes { get; set; }
        /// <summary>
        /// Output name for associated icon (with file extension)
        /// </summary>
        public string IconName { get; set; }
    }
}