using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.GUI.Windows;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for CreateAlarmItem.xaml
    /// </summary>
    public partial class CreateAlarmItem : UserControl
    {
        private static Bitmap _plusBitmap;

        #region properties

        [Category("Custom Props")]
        public ImageSource DynamicImagePlus { get; set; }

        #endregion
        public CreateAlarmItem()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            InitializeComponent();

            _plusBitmap =
                (Bitmap)Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Plus.png"));

            this.MouseLeftButtonUp += CreateAlarmItem_MouseLeftButtonUp;

            ColorStopwatch();
        }

        private void CreateAlarmItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StyleSettings ss = new StyleSettings();
            ss.Show();
        }

        public async void ColorStopwatch()
        {
            Bitmap source = _plusBitmap;

            SolidColorBrush brush = FindResource("StopwatchColor") as SolidColorBrush;

            var c = Color.FromArgb(255, 255, 255);
            if (brush != null)
            {
                var targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Bitmap bp = Dispatcher.InvokeAsync(() => source.ColorReplace(5, c, targetColor), DispatcherPriority.Render).Result;
                Bitmap bp = await Dispatcher.InvokeAsync(() => source.FastColorReplace(c, targetColor), DispatcherPriority.Render);

                Plus.Source = bp.ToBitmapImage();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            StyleSettings ss = new();

            ss.Show();
        }
    }
}
