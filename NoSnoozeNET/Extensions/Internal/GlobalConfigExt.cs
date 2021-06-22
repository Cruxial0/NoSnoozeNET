using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSnoozeNET.Config;

namespace NoSnoozeNET.Extensions.Internal
{
    public static class GlobalConfigExt
    {
        public static void AddTheme(this GlobalConfig globalConfig, UserTheme userTheme)
        {
            globalConfig.ThemeConfig.Remove(userTheme.ThemeName);
            globalConfig.ThemeConfig.Add(userTheme.ThemeName, userTheme.brushConfig);
        }
    }
}
