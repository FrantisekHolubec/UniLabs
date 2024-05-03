// Author: František Holubec
// Copyright (c) UniLabs

using System;

namespace UniLabs.Time
{
    public enum TimeUnit
    {
        None            = 0,
        Milliseconds    = 1,
        Seconds         = 2,
        Minutes         = 3,
        Hours           = 4,
        Days            = 5,
    }

    public static class TimeUnitExtensions
    {
        public delegate TimeSpan WithUnitValueDelegate(TimeSpan timeSpan, TimeUnit timeUnit, double value);
        public delegate double GetUnitValueDelegate(TimeSpan timeSpan, TimeUnit timeUnit);

        public static string ToShortString(this TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => "ms",
            TimeUnit.Seconds => "s",
            TimeUnit.Minutes => "m",
            TimeUnit.Hours => "h",
            TimeUnit.Days => "D",
            TimeUnit.None => "",
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null),
        };

        public static string ToSeparatorString(this TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => "",
            TimeUnit.Seconds => ".",
            TimeUnit.Minutes => ":",
            TimeUnit.Hours => ":",
            TimeUnit.Days => ".",
            TimeUnit.None => "",
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null),
        };

        public static double GetUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan.Milliseconds,
            TimeUnit.Seconds => timeSpan.Seconds,
            TimeUnit.Minutes => timeSpan.Minutes,
            TimeUnit.Hours => timeSpan.Hours,
            TimeUnit.Days => timeSpan.Days,
            TimeUnit.None => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static TimeSpan WithUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan.Add(TimeSpan.FromMilliseconds(value - timeSpan.Milliseconds)),
            TimeUnit.Seconds => timeSpan.Add(TimeSpan.FromSeconds(value - timeSpan.Seconds)),
            TimeUnit.Minutes => timeSpan.Add(TimeSpan.FromMinutes(value - timeSpan.Minutes)),
            TimeUnit.Hours => timeSpan.Add(TimeSpan.FromHours(value - timeSpan.Hours)),
            TimeUnit.Days => timeSpan.Add(TimeSpan.FromDays(value - timeSpan.Days)),
            TimeUnit.None => timeSpan,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static double GetLowestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan.Milliseconds,
            TimeUnit.Seconds => new TimeSpan(0,0,0, timeSpan.Seconds, timeSpan.Milliseconds).TotalSeconds,
            TimeUnit.Minutes => new TimeSpan(0,0, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).TotalMinutes,
            TimeUnit.Hours => new TimeSpan(0, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).TotalHours,
            TimeUnit.Days => timeSpan.TotalDays,
            TimeUnit.None => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static TimeSpan WithLowestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value) => timeUnit switch
        {
            TimeUnit.Milliseconds => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, (int) value),
            TimeUnit.Seconds => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0).Add(TimeSpan.FromSeconds(value)),
            TimeUnit.Minutes => new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0).Add(TimeSpan.FromMinutes(value)),
            TimeUnit.Hours => new TimeSpan(timeSpan.Days, 0, 0, 0).Add(TimeSpan.FromHours(value)),
            TimeUnit.Days => TimeSpan.FromDays(value),
            TimeUnit.None => timeSpan,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static double GetHighestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
            TimeUnit.Seconds => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).TotalSeconds,
            TimeUnit.Minutes => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0).TotalMinutes,
            TimeUnit.Hours => new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0).TotalHours,
            TimeUnit.Days => timeSpan.Days,
            TimeUnit.None => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static TimeSpan WithHighestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value) => timeUnit switch
        {
            TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(value),
            TimeUnit.Seconds => new TimeSpan(0,0,0,0, timeSpan.Milliseconds).Add(TimeSpan.FromSeconds(value)),
            TimeUnit.Minutes => new TimeSpan(0,0,0, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromMinutes(value)),
            TimeUnit.Hours => new TimeSpan(0,0, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromHours(value)),
            TimeUnit.Days => new TimeSpan(0, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromDays(value)),
            TimeUnit.None => timeSpan,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static double GetSingleUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
            TimeUnit.Seconds => timeSpan.TotalSeconds,
            TimeUnit.Minutes => timeSpan.TotalMinutes,
            TimeUnit.Hours => timeSpan.TotalHours,
            TimeUnit.Days => timeSpan.TotalDays,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static TimeSpan FromSingleUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value) => timeUnit switch
        {
            TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(value),
            TimeUnit.Seconds => TimeSpan.FromSeconds(value),
            TimeUnit.Minutes => TimeSpan.FromMinutes(value),
            TimeUnit.Hours => TimeSpan.FromHours(value),
            TimeUnit.Days => TimeSpan.FromDays(value),
            TimeUnit.None => TimeSpan.Zero,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };

        public static TimeSpan SnapToUnit(this TimeSpan timeSpan, TimeUnit timeUnit) => timeUnit switch
        {
            TimeUnit.Milliseconds => timeSpan,
            TimeUnit.Seconds => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds),
            TimeUnit.Minutes => new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0),
            TimeUnit.Hours => new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0),
            TimeUnit.Days => new TimeSpan(timeSpan.Days, 0, 0, 0),
            TimeUnit.None => timeSpan,
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
        };
    }
}
