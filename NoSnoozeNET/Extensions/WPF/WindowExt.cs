using NoSnoozeNET.Config;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;

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
            Color c = new Color();
            if (MainWindow.GlobalConfig.BrushConfig.MainBrush.ShadowColorBrush == null)
            {
                c = Colors.Black;
            }
            else
            {
                c = MainWindow.GlobalConfig.BrushConfig.MainBrush.ShadowColorBrush.Color;
            }


            uiElement.Effect = new DropShadowEffect()
            {
                BlurRadius = shadowConfig.BlurRadius,
                Color = c,
                Direction = shadowConfig.Direction,
                ShadowDepth = shadowConfig.ShadowDepth,
                Opacity = shadowConfig.Opacity
            };
        }
    }
}
