using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using NoSnoozeNET.Extensions.Imaging;
using NoSnoozeNET.Extensions.Internal;
using NoSnoozeNET.GUI.Functionality.Theme;
using Color = System.Windows.Media.Color;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for StyleSettings.xaml
    /// </summary>
    public partial class StyleSettings : Window
    {
        // Declare all UI elements in a static manner for global access.
        public static List<AlarmItem> PreviewItems = new List<AlarmItem>();
        public static UIElement ContainerElement;
        public static UIElement ThemesBoxElement;
        public static UIElement ShadowControlElement;

        private readonly List<UIElement> _shadowElements = new List<UIElement>();

        public StyleSettings()
        {
            InitializeComponent();
            Dispatcher.InvokeAsync(async () => await ColorImages(), DispatcherPriority.Render);
            CreatePreview();
            SetSliders();
            Dispatcher.InvokeAsync(async() => await Startup(), DispatcherPriority.Background);
        }

        public async Task Startup()
        {
            MainWindow.GlobalConfig.BuildThemeConfig();

            ThemePresetMethods.ApplyTheme(MainWindow.GlobalConfig.BrushConfig);
            WindowExt.Refresh(this);

            //Assign global UIElement variables.
            ContainerElement = this.ControlContainerBorder;
            ThemesBoxElement = this.cmbThemes;
            ShadowControlElement = this.ShadowControlBorder;

            //Add Elements to shadow into list.
            _shadowElements.Add(ContainerElement);
            _shadowElements.Add(ThemesBoxElement);
            _shadowElements.Add(ShadowControlElement);
            _shadowElements.Add(btnOpenFolder);
            _shadowElements.Add(btnSaveTheme);
            _shadowElements.Add(btnForceApply);
            _shadowElements.Add(btnAddTheme);

            //Add references from MainWindow
            _shadowElements.Add(MainWindow.AlarmListElement);
            _shadowElements.Add(MainWindow.TopBarElement);

            //Apply Shadows
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);

            //Sets ItemSource of ComboBox to loaded UserThemes.
            cmbThemes.ItemsSource = MainWindow.GlobalConfig.ThemeConfig.Keys;

            //Checks or Unchecks CheckBox according to UsingAdvanced Property.
            UseAdvanced.IsChecked = MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced;

            //Sets the selected theme as SelectedItem.
            cmbThemes.SelectedItem = MainWindow.GlobalConfig.SelectedTheme;
        }

        /// <summary>
        /// Sets color of all color pickers.
        /// </summary>
        public void SetColorPickers()
        {
            clrPrimary.SelectedColor = ((SolidColorBrush)Application.Current.Resources["LabelBrush"]).Color;
            clrSecondary.SelectedColor = ((SolidColorBrush)Application.Current.Resources["BorderBrush"]).Color;
            clrBackground.SelectedColor = ((SolidColorBrush)Application.Current.Resources["BackgroundBrush"]).Color;
            clrControlBg.SelectedColor = ((SolidColorBrush)Application.Current.Resources["ControlBackgroundBrush"]).Color;

            clrStopwatch.SelectedColor = ((SolidColorBrush)Application.Current.Resources["StopwatchColor"]).Color;
            clrOptions.SelectedColor = ((SolidColorBrush)Application.Current.Resources["OptionsColor"]).Color;
            clrDescription.SelectedColor = ((SolidColorBrush)Application.Current.Resources["DescriptionBrush"]).Color;
            clrHeader.SelectedColor = ((SolidColorBrush)Application.Current.Resources["HeaderBrush"]).Color;
        }

        public void SetSliders()
        {
            sldShadowBlurRadius.Value = MainWindow.GlobalConfig.BrushConfig.ShadowConfig.BlurRadius;
            sldShadowDirection.Value = MainWindow.GlobalConfig.BrushConfig.ShadowConfig.Direction;
            sldShadowOpacity.Value = MainWindow.GlobalConfig.BrushConfig.ShadowConfig.Opacity;
            sldShadowDepth.Value = MainWindow.GlobalConfig.BrushConfig.ShadowConfig.ShadowDepth;
        }

        private void CreatePreview()
        {
            //Make new list of AlarmItem.
            PreviewItems = new List<AlarmItem>();

            //Set source to newly generated list.
            ControlContainer.ItemsSource = PreviewItems;

            //Declare and populate new AlarmItem.
            AlarmItem a = new AlarmItem()
            {
                AlarmName = "Test Alarm",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings at 8PM (in 7h)",
            };

            //Scale AlarmItem by 0.75x.
            a.LayoutTransform = new ScaleTransform(0.75, 0.75);

            AlarmItem a2 = new AlarmItem()
            {
                AlarmName = "Straight buzzin'",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings at 9AM (in 18h)",
            };

            //Scale AlarmItem by 0.75x.
            a2.LayoutTransform = new ScaleTransform(0.75, 0.75);

            AlarmItem a3 = new AlarmItem()
            {
                AlarmName = "Straight buzzin'",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings at 9AM (in 18h)",
            };

            //Scale AlarmItem by 0.75x.
            a3.LayoutTransform = new ScaleTransform(0.75, 0.75);

            //Add AlarmItem to list.
            PreviewItems.Add(a);
            PreviewItems.Add(a2);
            PreviewItems.Add(a3);
        }

        private async Task ColorImages()
        {
            var brush = MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush;

            var source =
                (Bitmap)Image.FromFile(System.IO.Path.Combine(MainWindow.StartupDirectory, @"Assets\Plus.png"));

            var c = System.Drawing.Color.FromArgb(255, 255, 255);
            if (brush != null)
            {
                var targetColor = System.Drawing.Color.FromArgb(brush.Color.R, brush.Color.G, brush.Color.B);

                //Bitmap bp = Dispatcher.InvokeAsync(() => source.ColorReplace(5, c, targetColor), DispatcherPriority.Render).Result;
                Bitmap bp = await Dispatcher.InvokeAsync(() => source.FastColorReplace(c, targetColor), DispatcherPriority.Render);

                btnAddTheme.Source = bp.ToBitmapImage();
            }
        }

        //Region to group the CTRL+C/CTRL+V spam
        #region ControlSpam

        private void ClrPrimary_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.MainBrush.LabelBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrBackground_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.MainBrush.BackgroundColorBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrSecondary_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBorderBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrStopwatch_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            PreviewItems.First().ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush);
            BrushConfigMethods.ColorAlarmList(MainWindow.alarmList.ToList());
        }

        private void ClrOptions_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            PreviewItems.First().ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush);
            BrushConfigMethods.ColorAlarmList(MainWindow.alarmList.ToList());
        }

        private void ClrDescription_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.DescriptionBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrHeader_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.HeaderBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrControlBg_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.MainBrush.ControlBackgroundBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrShadow_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.MainBrush.ShadowColorBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        #endregion

        private void UseAdvanced_OnChecked(object sender, RoutedEventArgs e)
        {
            //Set UsingAdvanced property to check state.
            if (UseAdvanced.IsChecked != null)
                MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced = UseAdvanced.IsChecked.Value;

            //Show advanced Settings
            WindowAdvanced.Visibility = Visibility.Visible;
            WindowAdvanced.IsEnabled = true;

        }

        private void UseAdvanced_OnUnchecked(object sender, RoutedEventArgs e)
        {
            //Set UsingAdvanced property to check state.
            if (UseAdvanced.IsChecked != null)
                MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced = UseAdvanced.IsChecked.Value;

            //Hide advanced Settings
            WindowAdvanced.Visibility = Visibility.Hidden;
            WindowAdvanced.IsEnabled = false;
        }

        private async void CmbThemes_OnDropDownClosed(object sender, EventArgs e)
        {
            //Check that an item is selected.
            if (cmbThemes.SelectedIndex != -1)
            {
                //Build ThemeConfig.
                MainWindow.GlobalConfig.BuildThemeConfig();
                //Set selected theme to ComboBox value.
                MainWindow.GlobalConfig.SelectedTheme = (string)cmbThemes.SelectedValue;
                //Get value from Dictionary.
                MainWindow.GlobalConfig.ThemeConfig.TryGetValue((string)cmbThemes.SelectedValue, out var brushConfig);
                //Apply the theme.
                ThemePresetMethods.ApplyTheme(brushConfig);

                //Color preview item.
                await Dispatcher.InvokeAsync(() => PreviewItems.First().ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush), DispatcherPriority.Render);
                await Dispatcher.InvokeAsync(() => PreviewItems.First().ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush), DispatcherPriority.Render);

                //Refresh window
                WindowExt.Refresh(this);

                //Set color pickers accordingly.
                SetColorPickers();
                SetSliders();
                await ColorImages();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //Create new instance of SaveTheme, then show window.
            SaveTheme st = new SaveTheme();
            st.Show();
        }

        private void BtnOpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            //Open Theme Directory in File Explorer.
            Process.Start("explorer.exe", ThemeHandler.ThemeDirectory);
        }

        #region ShadowControl

        private void SldShadowDirection_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.GlobalConfig.BrushConfig.ShadowConfig.Direction = sldShadowDirection.Value;
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        private void SldShadowDepth_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.GlobalConfig.BrushConfig.ShadowConfig.ShadowDepth = sldShadowDepth.Value;
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        private void SldShadowBlurRadius_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.GlobalConfig.BrushConfig.ShadowConfig.BlurRadius = sldShadowBlurRadius.Value;
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        private void SldShadowOpacity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.GlobalConfig.BrushConfig.ShadowConfig.Opacity = sldShadowOpacity.Value;
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        #endregion


        private void BtnForceApply_OnClick(object sender, RoutedEventArgs e)
        {
            WindowExt.Refresh(this);
            WindowExt.Refresh(Application.Current.MainWindow);
            ShadowConfigExt.ApplyGlobalShadow(_shadowElements);
        }

        private async void StyleSettings_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetSliders();
            await ColorImages();
        }
    }
}
