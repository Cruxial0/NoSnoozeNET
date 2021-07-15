using System.Windows.Controls;
using System.Windows.Media;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for PluginListItem.xaml
    /// </summary>
    public partial class PluginListItem : UserControl
    {
        public Image PluginImage { get; set; }
        public bool Added { get; set; }

        public PluginListItem()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            PluginImage = new Image();

            Added = false;
        }

        public bool Toggle()
        {
            if (Added)
            {
                Added = false;
                return Added;
            }

            Added = true;
            return Added;
        }
    }
}
