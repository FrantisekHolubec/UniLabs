// Author: František Holubec
// Created: 03.05.2024
// Copyright (c) Noxgames
// http://www.noxgames.com/

using System;
using System.Diagnostics;

namespace UniLabs.Time
{
    [Conditional("UNITY_EDITOR")]
    public class TimeSpanDrawerSettingsAttribute : Attribute
    {
        public TimeUnit HighestUnit = TimeUnit.Days;
        public TimeUnit LowestUnit = TimeUnit.Seconds;

        public TimeSpanDrawerSettingsAttribute() { }

        public TimeSpanDrawerSettingsAttribute(TimeUnit highestUnit, TimeUnit lowestUnit)
        {
            HighestUnit = highestUnit;
            LowestUnit = lowestUnit;
        }

        public TimeSpanDrawerSettingsAttribute(TimeUnit highestUnit, bool drawMilliseconds = false)
        {
            HighestUnit = highestUnit;
            LowestUnit = drawMilliseconds ? TimeUnit.Milliseconds : TimeUnit.Seconds;
        }

        public TimeSpanDrawerSettingsAttribute(bool drawMilliseconds)
        {
            HighestUnit = TimeUnit.Days;
            LowestUnit = drawMilliseconds ? TimeUnit.Milliseconds : TimeUnit.Seconds;
        }
    }
}
