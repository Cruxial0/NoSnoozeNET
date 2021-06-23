﻿using System;
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
        public static void ApplyGlobalShadow()
        {
            StyleSettings ST = new StyleSettings();
            var mw = (MainWindow)Application.Current.MainWindow;

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, StyleSettings.ContainerElement);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, StyleSettings.ThemesBoxElement);
            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, StyleSettings.ShadowControlElement);

            WindowExt.ApplyShadow(MainWindow.GlobalConfig.BrushConfig.ShadowConfig, mw.AlarmList);
        }
    }
}