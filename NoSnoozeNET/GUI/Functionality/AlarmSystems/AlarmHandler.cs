using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using NoSnoozeNET.GUI.Controls;
using NoSnoozeNET.GUI.Functionality.PluginSystems;
using NoSnoozeNET.PluginSystem;

namespace NoSnoozeNET.GUI.Functionality.AlarmSystems
{
    public class AlarmHandler
    {
        private DispatcherTimer _activeTimer = null;
        private AlarmItem _ringingItem = null;

        private void PerformRing(AlarmItem alarmItem)
        {
            //Dynamic Pop-up
            //Ring sound
        }

        public void DetermineRing(AlarmItem alarmItem)
        {
            var now = DateTime.Now;
            var secondsNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var secondsTarget = new DateTime(alarmItem.RingsAt.Year, alarmItem.RingsAt.Month, alarmItem.RingsAt.Day, alarmItem.RingsAt.Hour, alarmItem.RingsAt.Minute, alarmItem.RingsAt.Second);

            if (TimeSpan.Compare(secondsNow.TimeOfDay, secondsTarget.TimeOfDay) == 0 && !alarmItem.IsPreviewItem)
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
        }

        private void OffsetTimer_Tick(object sender, EventArgs e)
        {
            DetermineRing(_ringingItem);
            _activeTimer.Stop();
            _activeTimer = null;
        }
    }
}
