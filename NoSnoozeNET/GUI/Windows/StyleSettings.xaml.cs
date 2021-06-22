using NoSnoozeNET.Config;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using NoSnoozeNET.GUI.Functionality.Theme;

namespace NoSnoozeNET.GUI.Windows
{
    /// <summary>
    /// Interaction logic for StyleSettings.xaml
    /// </summary>
    public partial class StyleSettings : Window
    {
        private List<AlarmItem> previewItem = new List<AlarmItem>();
        public StyleSettings()
        {
            InitializeComponent();
            CreatePreview();

            cmbThemes.ItemsSource = MainWindow.GlobalConfig.ThemeConfig.Keys;

            UseAdvanced.IsChecked = MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced;

            cmbThemes.SelectedItem = MainWindow.GlobalConfig.SelectedTheme;

        }

        public void SetColorPickers()
        {
            clrPrimary.SelectedColor = ((SolidColorBrush)Application.Current.Resources["LabelBrush"]).Color;
            clrSecondary.SelectedColor = ((SolidColorBrush)Application.Current.Resources["SecondaryBrush"]).Color;
            clrBackground.SelectedColor = ((SolidColorBrush)Application.Current.Resources["BackgroundBrush"]).Color;
            clrControlBg.SelectedColor = ((SolidColorBrush)Application.Current.Resources["ControlBackgroundBrush"]).Color;

            clrStopwatch.SelectedColor = ((SolidColorBrush)Application.Current.Resources["StopwatchColor"]).Color;
            clrOptions.SelectedColor = ((SolidColorBrush)Application.Current.Resources["OptionsColor"]).Color;
            clrDescription.SelectedColor = ((SolidColorBrush)Application.Current.Resources["DescriptionBrush"]).Color;
            clrHeader.SelectedColor = ((SolidColorBrush)Application.Current.Resources["HeaderBrush"]).Color;
        }

        private void CreatePreview()
        {
            previewItem = new List<AlarmItem>();

            ControlContainer.ItemsSource = previewItem;

            Size half = new Size(220, 52.5);

            AlarmItem a = new AlarmItem()
            {
                AlarmName = "Test Alarm",
                AlarmCreated = "Created: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeToRing = "Rings at 8PM (in 7h)",
            };

            a.LayoutTransform = new ScaleTransform(0.75, 0.75);

            previewItem.Add(a);
        }

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
            MainWindow.GlobalConfig.BrushConfig.MainBrush.SecondaryColorBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();
        }

        private void ClrStopwatch_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            previewItem.First().ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush);
            BrushConfigMethods.ColorAlarmList(MainWindow.alarmList.ToList());
        }

        private void ClrOptions_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush = new SolidColorBrush(e.NewValue.Value);
            MainWindow.GlobalConfig.BrushConfig.ApplyBrushConfig();

            previewItem.First().ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush);
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

        private void UseAdvanced_OnChecked(object sender, RoutedEventArgs e)
        {
            if (UseAdvanced.IsChecked != null)
                MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced = UseAdvanced.IsChecked.Value;

            WindowAdvanced.Visibility = Visibility.Visible;
            WindowAdvanced.IsEnabled = true;

        }

        private void UseAdvanced_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (UseAdvanced.IsChecked != null)
                MainWindow.GlobalConfig.BrushConfig.MainBrush.UsingAdvanced = UseAdvanced.IsChecked.Value;

            WindowAdvanced.Visibility = Visibility.Hidden;
            WindowAdvanced.IsEnabled = false;
        }

        private async void CmbThemes_OnDropDownClosed(object sender, EventArgs e)
        {
            if (cmbThemes.SelectedIndex != -1)
            {
                MainWindow.GlobalConfig.BuildThemeConfig();
                MainWindow.GlobalConfig.SelectedTheme = (string)cmbThemes.SelectedValue;
                MainWindow.GlobalConfig.ThemeConfig.TryGetValue((string)cmbThemes.SelectedValue, out var brushConfig);
                ThemePresetMethods.ApplyTheme(brushConfig);

                await Dispatcher.InvokeAsync(() => previewItem.First().ColorStopwatch(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.StopwatchBrush), DispatcherPriority.Render);
                await Dispatcher.InvokeAsync(() => previewItem.First().ColorOptions(MainWindow.GlobalConfig.BrushConfig.AlarmItemBrush.OptionsBrush), DispatcherPriority.Render);

                WindowExt.Refresh(this);

                SetColorPickers();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SaveTheme st = new SaveTheme();
            st.Show();
        }

        private void BtnOpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", ThemeHandler.ThemeDirectory);
        }
    }
}
