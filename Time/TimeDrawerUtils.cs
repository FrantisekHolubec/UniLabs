// Author: František Holubec
// Copyright (c) UniLabs

#if UNITY_EDITOR
using System;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UniLabs.Time
{
    public static class TimeDrawerUtils
    {
        private static readonly TimeUnit[] ORDERED_UNITS =
        {
            TimeUnit.Days,
            TimeUnit.Hours,
            TimeUnit.Minutes,
            TimeUnit.Seconds,
            TimeUnit.Milliseconds
        };

        public static TimeSpan DrawTimeSpanField(GUIContent label, TimeSpan currentValue, TimeUnit highestUnit = TimeUnit.Days, TimeUnit lowestUnit = TimeUnit.Seconds)
        {
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);

            if (lowestUnit == highestUnit)
            {
                currentValue = DrawTimeUnitSingleField(currentValue, lowestUnit);
            }
            else
            {
                foreach (var unit in ORDERED_UNITS)
                {
                    if (highestUnit < unit || unit < lowestUnit)
                        continue;

                    if (unit == lowestUnit) currentValue = DrawTimeUnitLowestField(currentValue, unit);
                    else if (unit == highestUnit) currentValue = DrawTimeUnitHighestField(currentValue, unit);
                    else currentValue = DrawTimeUnitField(currentValue, unit);

                    if (unit != lowestUnit)
                    {
                        EditorGUILayout.LabelField(unit.ToSeparatorString(), GUILayout.Width(5));
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalPropertyLayout();
            return currentValue;
        }

        private static TimeSpan DrawTimeUnitField(TimeSpan currentValue, TimeUnit unit, TimeUnitExtensions.GetUnitValueDelegate getter, TimeUnitExtensions.WithUnitValueDelegate setter)
        {
            var value = getter.Invoke(currentValue, unit);
            var newValue = EditorGUILayout.FloatField((float) value);

            var prefixLabel = unit.ToShortString();
            GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), prefixLabel, SirenixGUIStyles.RightAlignedGreyMiniLabel);

            return Math.Abs(newValue - value) > 0.0001 ? setter.Invoke(currentValue, unit, newValue) : currentValue;
        }

        private static TimeSpan DrawTimeUnitField(TimeSpan currentValue, TimeUnit unit) =>
            DrawTimeUnitField(currentValue, unit, TimeUnitExtensions.GetUnitValue, TimeUnitExtensions.WithUnitValue);

        private static TimeSpan DrawTimeUnitLowestField(TimeSpan currentValue, TimeUnit unit) =>
            DrawTimeUnitField(currentValue, unit, TimeUnitExtensions.GetLowestUnitValue, TimeUnitExtensions.WithLowestUnitValue);

        private static TimeSpan DrawTimeUnitHighestField(TimeSpan currentValue, TimeUnit unit) =>
            DrawTimeUnitField(currentValue, unit, TimeUnitExtensions.GetHighestUnitValue, TimeUnitExtensions.WithHighestUnitValue);

        private static TimeSpan DrawTimeUnitSingleField(TimeSpan currentValue, TimeUnit unit) =>
            DrawTimeUnitField(currentValue, unit, TimeUnitExtensions.GetSingleUnitValue, TimeUnitExtensions.FromSingleUnitValue);

        public static TimeSpan DrawTimeSpanSlider(TimeSpan value, TimeSpan min, TimeSpan max, TimeUnit snapping)
        {
            var rect = EditorGUILayout.GetControlRect(false);
            var totalSeconds = GUI.HorizontalSlider(rect, (float) value.TotalSeconds, (float) min.TotalSeconds, (float) max.TotalSeconds);
            return TimeSpan.FromSeconds(totalSeconds).SnapToUnit(snapping);
        }

        public static Tuple<TimeSpan, TimeSpan> DrawTimeSpanMinMaxSlider(TimeSpan start, TimeSpan end, TimeSpan min, TimeSpan max, TimeUnit snapping)
        {
            var startSeconds = (float) start.TotalSeconds;
            var endSeconds = (float) end.TotalSeconds;

            EditorGUILayout.MinMaxSlider(ref startSeconds, ref endSeconds, (float) min.TotalSeconds, (float) max.TotalSeconds);

            start = TimeSpan.FromSeconds(startSeconds).SnapToUnit(snapping);
            end = TimeSpan.FromSeconds(endSeconds).SnapToUnit(snapping);

            return new Tuple<TimeSpan, TimeSpan>(start, end);
        }

        public static void DrawDateTimeField(GUIContent label, DateTime currentValue, Action<DateTime> valueSetter)
        {
            var day = currentValue.Day;
            var month = currentValue.Month;
            var year = currentValue.Year;
            var hour = currentValue.Hour;
            var min = currentValue.Minute;
            var sec = currentValue.Second;
            var milliSec = currentValue.Millisecond;

            var verticalRect = SirenixEditorGUI.BeginVerticalPropertyLayout(label);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                day = SirenixEditorFields.IntField(day);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "D", SirenixGUIStyles.RightAlignedGreyMiniLabel);
                EditorGUILayout.LabelField(".", GUILayout.Width(5));
                month = SirenixEditorFields.IntField(month);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "M", SirenixGUIStyles.RightAlignedGreyMiniLabel);
                EditorGUILayout.LabelField(".", GUILayout.Width(5));
                year = SirenixEditorFields.IntField(year);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "Y", SirenixGUIStyles.RightAlignedGreyMiniLabel);

                var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button,  GUILayoutOptions.ExpandWidth(false).Width(18));
                if (SirenixEditorGUI.IconButton(rect, EditorIcons.DayCalendar))
                {
                    var dateTimePicker = new DateTimePicker(currentValue, dateTime =>
                    {
                        valueSetter?.Invoke(dateTime);
                    });
                    dateTimePicker.ShowInPopup(rect);
                }

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                hour = SirenixEditorFields.IntField(hour);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "h", SirenixGUIStyles.RightAlignedGreyMiniLabel);
                EditorGUILayout.LabelField(":", GUILayout.Width(5));
                min = SirenixEditorFields.IntField(min);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "m", SirenixGUIStyles.RightAlignedGreyMiniLabel);
                EditorGUILayout.LabelField(":", GUILayout.Width(5));
                sec = SirenixEditorFields.IntField(sec);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "s", SirenixGUIStyles.RightAlignedGreyMiniLabel);
                EditorGUILayout.LabelField(".", GUILayout.Width(5));
                milliSec = SirenixEditorFields.IntField(milliSec);
                GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "ms", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(1);
            if (EditorGUI.EndChangeCheck())
            {
                try
                {
                    valueSetter?.Invoke(new DateTime(year, month, day, hour, min, sec, milliSec));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            SirenixEditorGUI.EndVerticalPropertyLayout();
        }
    }
}
#endif
