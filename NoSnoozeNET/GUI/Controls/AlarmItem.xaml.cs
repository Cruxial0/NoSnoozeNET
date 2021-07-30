using System;
using System.Collections.Generic;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.GUI.Windows;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NoSnoozeNET.PluginSystem;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace NoSnoozeNET.GUI.Controls
{
    /// <summary>
    /// Interaction logic for AlarmItem.xaml
    /// </summary>
    public partial class AlarmItem : UserControl, INotifyPropertyChanged
    {
        //Declare inheritants
        private static Bitmap _stopwatchBitmap;
        private static Bitmap _optionsBitmap;
        private DateTime _ringsAt;
        private List<Plugin> _pluginElements;
        private string _alarmName;

        //Declare local image variables
        private BitmapImage _stopwatchImageSource;
        private BitmapImage _optionsImageSource;

        #region properties


        public bool IsPreviewItem = false;


        [Category("Custom Props")]
        public string AlarmName
        {
            get => _alarmName;
            set { _alarmName = value; NotifyPropertyChanged(nameof(AlarmName)); }
        }

        [Category("Custom Props")]
        public string AlarmCreated
        {
            get => (string)lblCreatedAt.Content;
            set { lblCreatedAt.Content = value; NotifyPropertyChanged(nameof(AlarmCreated)); }
        }

        [Category("Custom Props")]
        public string TimeToRing
        {
            get => (string)lblRingsAt.Content;
            set { lblRingsAt.Content = value; NotifyPropertyChanged(nameof(TimeToRing)); }
        }

        public DateTime RingsAt
        {
            get => _ringsAt;
            set { _ringsAt = value; NotifyPropertyChanged(nameof(RingsAt)); }
        }

        public List<Plugin> PluginElements
        {
            get => _pluginElements;
            set { _pluginElements = value; NotifyPropertyChanged(nameof(PluginElements)); }
        }

        public BitmapImage StopwatchImageSource
        {
            get => _stopwatchImageSource;
            set { _stopwatchImageSource = value; NotifyPropertyChanged(nameof(StopwatchImageSource)); }
        }

        public BitmapImage OptionsImageSource
        {
            get => _optionsImageSource;
            set { _optionsImageSource = value; NotifyPropertyChanged(nameof(OptionsImageSource)); }
        }
        #endregion

        public AlarmItem()
        {
            InitializeComponent();

            //Pre-define margin.
            this.Margin = new Thickness(0, 5, 0, 5);
            //Change BitmapScalingMode to HighQuality.
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            //Find Stopwatch image asset.
            _stopwatchBitmap =
                (Bitmap)Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Stopwatch.png"));

            //Find Options image asset.
            _optionsBitmap =
                (Bitmap) Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Options.png"));

            //Color assets with Application Resources.
            ColorStopwatch(FindResource("StopwatchColor") as SolidColorBrush);
            ColorOptions(FindResource("OptionsColor") as SolidColorBrush);

            _pluginElements = new List<Plugin>();

        }

        public async void ColorStopwatch(SolidColorBrush brush)
        {
            //Make new instance Stopwatch Image to prevent mutability and coloring issues.
            Bitmap source = _stopwatchBitmap;

            //Make sure brush isn't null.
            if (brush != null)
            {
                //Get the System.Drawing.Color equivalent of Application Resource brush.
                var targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Dispatch task to separate thread, then recolor image with Render DispatcherPriority.
                source = await Dispatcher.InvokeAsync(() => source.FastColorReplace(Color.White, targetColor), DispatcherPriority.Render);

                //Convert Bitmap to BitmapImage and set it as ImageSource.
                //Stopwatch.Source = source.ToBitmapImage();
                StopwatchImageSource = source.ToBitmapImage();
            }
        }

        public void InitializePlugins()
        {
            foreach (var plugin in PluginElements)
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image
                {
                    Source = ((Bitmap) ImageExt.ByteArrayToImage(plugin.PluginInfo.PluginIconInfo.IconBytes)).ToBitmapImage()
                };
                PluginPanel.Children.Add(img);
            }
        }

        public void AddPlugin(UIElement img, Plugin plugin)
        {
            PluginElements.Add(plugin);
            PluginPanel.Children.Add(img);
        }

        public void RemovePlugin(UIElement img, Plugin plugin)
        {
            PluginElements.Remove(plugin);
            PluginPanel.Children.Remove(img);
        } 

        public async void ColorOptions(SolidColorBrush brush)
        {
            //Make new instance Options Image to prevent mutability and coloring issues.
            Bitmap source = _optionsBitmap;

            //Make sure brush isn't null.
            if (brush != null)
            {
                //Get the System.Drawing.Color equivalent of Application Resource brush.
                Color targetColor = Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Dispatch task to separate thread, then recolor image with Render DispatcherPriority.
                source = await Dispatcher.InvokeAsync(() => source.FastColorReplace(Color.White, targetColor), DispatcherPriority.Render);

                //Convert Bitmap to BitmapImage and set it as ImageSource.
                //OptionsImg.Source = source.ToBitmapImage();
                OptionsImageSource = source.ToBitmapImage();
            }
        }

        private void BtnOptions_OnClick(object sender, RoutedEventArgs e)
        {
            //DO STUFF

            //Placeholder
            
            StyleSettings ss = new();
            ss.Show();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }

        private void AlarmItem_OnLoaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"{_alarmName}: {_ringsAt}");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();

            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var secondsNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var secondsTarget = new DateTime(RingsAt.Year, RingsAt.Month, RingsAt.Day, RingsAt.Hour, RingsAt.Minute, RingsAt.Second);

            if (TimeSpan.Compare(secondsNow.TimeOfDay, secondsTarget.TimeOfDay) == 0 && !IsPreviewItem)
            {
                foreach (var plugin in PluginElements)
                {
                    PluginLoader.Plugins.First(x => x.Name == plugin.PluginInfo.PluginName).Execute(new string[0]);
                }

                SoundPlayer player = new SoundPlayer(@"C:\Users\Benjamin\OneDrive\Documents\Audacity\歌ってみたKING 百鬼あやめ cover.wav");
                player.Load();
                player.Play();
            }
        }
    }
}
