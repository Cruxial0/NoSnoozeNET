using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NoSnoozeNET.Extensions.WPF;
using NoSnoozeNET.GUI.Windows;

namespace NoSnoozeNET.Extensions.Internal
{
    public static class ShadowConfigExt
    {
        public static void ApplyGlobalShadow(List<UIElement> shadowElements)
        {
            foreach (var element in shadowElements)
            {
                WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, element);
            }
        }
    }
}
