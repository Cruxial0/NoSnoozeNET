using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.GUI.Windows;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for AlarmItem.xaml
    /// </summary>
    public partial class AlarmItem : UserControl
    {
        private static Bitmap _stopwatchBitmap;
        private static Bitmap _optionsBitmap;

        #region properties

        [Category("Custom Props")]
        public string AlarmName { get; set; }

        [Category("Custom Props")]
        public string AlarmCreated { get; set; }

        [Category("Custom Props")]
        public string TimeToRing { get; set; }

        [Category("Custom Props")]
        public ImageSource DynamicImage { get; set; }

        [Category("Custom Props")]
        public ImageSource DynamicImageOptions { get; set; }
        #endregion

        public AlarmItem()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            _stopwatchBitmap =
                (Bitmap)Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Stopwatch.png"));

            _optionsBitmap =
                (Bitmap)Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Options.png"));

            ColorStopwatch(FindResource("StopwatchColor") as SolidColorBrush);
            ColorOptions(FindResource("OptionsColor") as SolidColorBrush);
        }

        public async void ColorStopwatch(SolidColorBrush brush)
        {
            Bitmap source = _stopwatchBitmap;


            var c = Color.FromArgb(255, 255, 255);
            if (brush != null)
            {
                var targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Bitmap bp = Dispatcher.InvokeAsync(() => source.ColorReplace(5, c, targetColor), DispatcherPriority.Render).Result;
                Bitmap bp = await Dispatcher.InvokeAsync(() => source.FastColorReplace(c, targetColor), DispatcherPriority.Render);

                Stopwatch.Source = bp.ToBitmapImage();
            }
        }

        public async void ColorOptions(SolidColorBrush brush)
        {
            Bitmap source = _optionsBitmap;

            Color c = Color.FromArgb(255, 255, 255);
            if (brush != null)
            {
                Color targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Bitmap bp = Dispatcher.InvokeAsync(() => source.ColorReplace(5, c, targetColor), DispatcherPriority.Render).Result;
                Bitmap bp = await Dispatcher.InvokeAsync(() => source.FastColorReplace(c, targetColor), DispatcherPriority.Render);

                OptionsImg.Source = bp.ToBitmapImage();
            }
        }

        private void BtnOptions_OnClick(object sender, RoutedEventArgs e)
        {
            StyleSettings ss = new();
            ss.Show();
        }
    }
}
