using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NoSnoozeNET.Annotations;
using NoSnoozeNET.Extensions.Imaging;
using Color = System.Drawing.Color;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for PluginListItem.xaml
    /// </summary>
    public partial class PluginListItem : UserControl, INotifyPropertyChanged
    {
        private System.Drawing.Image _pluginImage;
        private string _pluginName;

        public System.Drawing.Image PluginImage
        {
            get => _pluginImage;
            set { _pluginImage = value; OnPropertyChanged(nameof(PluginImage)); }
        }

        public string PluginName
        {
            get => _pluginName;
            set { _pluginName = value; OnPropertyChanged(nameof(PluginName)); }
        }
        public bool Added { get; set; }

        public PluginListItem()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            Added = false;
        }

        public void SetImage()
        {
            Image.Source = ((Bitmap)_pluginImage).ToBitmapImage();
        }

        private void SetImage(BitmapImage img)
        {
            Image.Source = img;
        }

        public async void ColorImage(SolidColorBrush brush)
        {
            Bitmap source = PluginImage as Bitmap;

            //Make sure brush isn't null.
            if (brush != null)
            {
                //Get the System.Drawing.Color equivalent of Application Resource brush.
                Color targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Dispatch task to separate thread, then recolor image with Render DispatcherPriority.
                source = await Dispatcher.InvokeAsync(() => source.FastColorReplace(Color.White, targetColor), DispatcherPriority.Render);

                SetImage(source.ToBitmapImage());
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
