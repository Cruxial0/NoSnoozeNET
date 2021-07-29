using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSnoozeNET.PluginSystem.Interfaces
{
    /// <summary>
    /// Interface for storing multiple data types.
    /// </summary>
    public interface ISnoozePluginConfig { }

    public class PluginConfig : ISnoozePluginConfig
    {
        public Dictionary<string, string> StringConfig { get; set; }
        public Dictionary<string, int> IntConfig { get; set; }
        public Dictionary<string, DateTime> DateTimeConfig { get; set; }
        public Dictionary<string, bool> BoolConfig { get; set; }

        public PluginConfig()
        {
            this.StringConfig = new();
            this.IntConfig = new();
            this.DateTimeConfig = new Dictionary<string, DateTime>();
            this.BoolConfig = new Dictionary<string, bool>();
        }
    }
}
