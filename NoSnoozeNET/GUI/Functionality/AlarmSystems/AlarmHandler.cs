using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.GUI.Functionality.PluginSystems;
using NoSnoozeNET.GUI.Windows;
using NoSnoozeNET.PluginSystem;

namespace NoSnoozeNET.GUI.Functionality.AlarmSystems
{
    public class AlarmHandler
    {
        private DispatcherTimer _activeTimer = null;
        private AlarmItem _ringingItem = null;

        private void PerformRing(AlarmItem alarmItem)
        {
            MessageBox.Show($"{alarmItem.AlarmName} rang at {DateTime.Now:HH:mm:ss tt}");

            PostRingSummary postRingSummary = new PostRingSummary(alarmItem);
            postRingSummary.Show();
            //Ring sound
        }

        public void DetermineRing(AlarmItem alarmItem)
        {
            HandlePlugins HP = new HandlePlugins();
            if(!HP.CheckConditions(alarmItem.PluginElements))
            {
                DispatcherTimer offsetTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(1000)
                };

                offsetTimer.Start();
                offsetTimer.Tick += OffsetTimer_Tick;

                _activeTimer = offsetTimer;
                _ringingItem = alarmItem;

                return;
            }

            PerformRing(alarmItem);
        }

        private void OffsetTimer_Tick(object sender, EventArgs e)
        {
            DetermineRing(_ringingItem);
            _activeTimer.Stop();
            _activeTimer = null;
        }
    }
}
