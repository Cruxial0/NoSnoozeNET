﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoSnoozeNET.Extensions.IO;
using NoSnoozeNET.GUI.Controls;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace NoSnoozeNET.Config
{
    public static class AlarmConfig
    {
        public static readonly string AlarmConfigDirectoryPath = Path.Combine(MainWindow.BinDirectory, @"Config\AlarmConfig");
        public static readonly string AlarmConfigPath = Path.Combine(MainWindow.BinDirectory, @"Config\AlarmConfig\AlarmConfig.json");

        public static void Save(this IEnumerable<AlarmItem> alarmItems)
        {
            DirectoryExt.CreateFolderIfNotExist(AlarmConfigDirectoryPath);
            DirectoryExt.CreateIfNotExist(AlarmConfigPath);

            //Prevent multiple enumerations.
            var items = alarmItems as AlarmItem[] ?? alarmItems.ToArray();

            if (alarmItems == null || !items.Any()) return;

            File.WriteAllText(AlarmConfigPath, JsonConvert.SerializeObject(items.ToAlarmInfo(), Formatting.Indented));
        }

        public static ObservableCollection<AlarmItem> Load(this IEnumerable<AlarmItem> alarmItems)
        {
            DirectoryExt.CreateFolderIfNotExist(AlarmConfigDirectoryPath);
            DirectoryExt.CreateIfNotExist(AlarmConfigPath);

            var schema = NJsonSchema.JsonSchema.FromType<IEnumerable<AlarmItem>>();

            var errors = schema.Validate(File.ReadAllText(AlarmConfigPath));

            //if (errors.Count != 0) return null;

            List<AlarmInfo> alarmInfo = (JsonConvert.DeserializeObject<IEnumerable<AlarmInfo>>(File.ReadAllText(AlarmConfigPath)) ?? Array.Empty<AlarmInfo>()).ToList();

            var outAlarmList = new ObservableCollection<AlarmItem>();

            foreach (var info in alarmInfo)
            {
                outAlarmList.Add(new AlarmItem()
                {
                    AlarmCreated = info.CreatedAt,
                    AlarmName = info.AlarmName,
                    RingsAt = info.RingsAt,
                    TimeToRing = DetermineNextRing(info.RingsAt)
                });
            }

            return outAlarmList;
        }

        private static List<AlarmInfo> ToAlarmInfo(this IEnumerable<AlarmItem> alarmItems)
        {
            List<AlarmInfo> infoList = new List<AlarmInfo>();
            foreach (var alarmItem in alarmItems)
            {
                var alarmInfo = new AlarmInfo()
                {
                    AlarmName = alarmItem.AlarmName,
                    CreatedAt = alarmItem.AlarmCreated,
                    RingsAt = alarmItem.RingsAt, 
                };

                infoList.Add(alarmInfo);
            }

            return infoList;
        }

        private static string DetermineNextRing(DateTime nextRing)
        {
            var now = DateTime.Now;
            double totalHours = 0;
            if (now > nextRing)
            {
                var tomorrow = now.AddDays(1).Date.AddHours(int.Parse(nextRing.ToString("hh")));
                totalHours = (tomorrow - now).TotalHours;
            }
            else
            {
                totalHours = (nextRing - now).TotalHours;
            }

            return $"Rings at {nextRing:HH:mm}";
        }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class AlarmInfo
    {
        public string AlarmName { get; set; }
        public string CreatedAt { get; set; }
        public DateTime RingsAt { get; set; }

        public AlarmInfo()
        {
            this.AlarmName = AlarmName;
            this.RingsAt = RingsAt;
            this.CreatedAt = CreatedAt;
        }
    }
}
