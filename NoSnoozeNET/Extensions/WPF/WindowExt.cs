using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using NoSnoozeNET.Config;

namespace NoSnoozeNET.Extensions.WPF
{
    class WindowExt
    {
        private static readonly Action EmptyDelegate = delegate () { };

        public static void Refresh(UIElement uiElement)
        {
            //Refresh element visuals.
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);

            //Perform Garbage Collection.
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public static void ApplyShadow(ShadowConfig shadowConfig, UIElement uiElement)
        {
            uiElement.Effect = new DropShadowEffect()
            {
                BlurRadius = shadowConfig.BlurRadius,
                Color = Colors.Black, //MainWindow.GlobalConfig.BrushConfig.MainBrush.ShadowColorBrush.Color,
                Direction = shadowConfig.Direction,
                ShadowDepth = shadowConfig.ShadowDepth,
                Opacity = shadowConfig.Opacity
            };
        }
    }
}
