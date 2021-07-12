using System.Windows.Controls;
using System.Windows.Media;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for PluginListItem.xaml
    /// </summary>
    public partial class PluginListItem : UserControl
    {
        public PluginListItem()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
        }
    }
}
