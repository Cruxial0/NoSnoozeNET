using Newtonsoft.Json;
using NoSnoozeNET.Extensions.IO;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace NoSnoozeNET.Config
{

    public class BrushConfig
    {
        public static readonly string ConfigPath = Path.Combine(MainWindow.BinDirectory, @"Config\GlobalConfig\BrushConfig.json");

        public MainBrush MainBrush { get; set; }
        public AlarmItemBrush AlarmItemBrush { get; set; }
        public ShadowConfig ShadowConfig { get; set; }

        public void SaveConfig()
        {
            DirectoryExt.CreateIfNotExist(ConfigPath);

            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public BrushConfig()
        {
            this.AlarmItemBrush = new AlarmItemBrush();
            this.MainBrush = new MainBrush();
            this.ShadowConfig = new ShadowConfig();
        }
    }

    /// <summary>
    /// All brushes used for AlarmItem.xaml
    /// </summary>
    public class AlarmItemBrush
    {
        public SolidColorBrush StopwatchBrush { get; set; }
        public SolidColorBrush HeaderBrush { get; set; }
        public SolidColorBrush DescriptionBrush { get; set; }
        public SolidColorBrush OptionsBrush { get; set; }
    }

    public class MainBrush
    {
        public bool UsingAdvanced { get; set; }
        public SolidColorBrush LabelBrush { get; set; }
        public SolidColorBrush SecondaryColorBrush { get; set; }
        public SolidColorBrush BackgroundColorBrush { get; set; }
        public SolidColorBrush ControlBackgroundBrush { get; set; }
        public SolidColorBrush ShadowColorBrush { get; set; }
    }
}
