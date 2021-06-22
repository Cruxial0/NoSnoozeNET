using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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

        
    }
}
