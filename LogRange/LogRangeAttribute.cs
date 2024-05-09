// Author: František Holubec
// Copyright (c) UniLabs

using System;
using System.Diagnostics;

namespace UniLabs.LogRange
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class LogRangeAttribute : Attribute
    {
        public float Min;
        public float Max;
        public float Center;

        public string MinGetter;
        public string MaxGetter;
        public string CenterGetter;

        public LogRangeAttribute(float min, float center, float max)
        {
            Min = min;
            Center = center;
            Max = max;
        }

        public LogRangeAttribute(string minGetter, string centerGetter, string maxGetter)
        {
            MinGetter = minGetter;
            CenterGetter = centerGetter;
            MaxGetter = maxGetter;
        }
    }
}
