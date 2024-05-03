// Author: František Holubec
// Copyright (c) UniLabs

#if UNITY_EDITOR
using System;
using System.Globalization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UniLabs.Utilities;
using UnityEditor;
using UnityEngine;

namespace UniLabs.Time
{
    public class DateTimePicker
    {
        public DateTime CurrentValue { get; set; }

        public Action<DateTime> OnValueChanged;
            
        private static readonly DayOfWeek[] WEEK_DAYS = {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday};

        private const float WINDOW_WIDTH = 245;
        
        public DateTimePicker(DateTime value, Action<DateTime> onValueChanged)
        {
            CurrentValue = value;
            OnValueChanged = onValueChanged;
        }

        public OdinEditorWindow ShowInPopup()
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, WINDOW_WIDTH);
        }

        public OdinEditorWindow ShowInPopup(Rect buttonRect)
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, buttonRect, WINDOW_WIDTH);
        }

        public OdinEditorWindow ShowInPopup(Vector2 windowPosition)
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, windowPosition, WINDOW_WIDTH);
        }
        
        [OnInspectorGUI]
        private void Draw()
        {
            var culture = new CultureInfo("en-US", false);
            
            var day = CurrentValue.Day;
            var month = CurrentValue.Month;
            var year = CurrentValue.Year;
            var hour = CurrentValue.Hour;
            var min = CurrentValue.Minute;
            var sec = CurrentValue.Second;
            var milliSec = CurrentValue.Millisecond;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            day = SirenixEditorFields.IntField(day);
            GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "D", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            EditorGUILayout.LabelField(".", GUILayout.Width(5));
            month = SirenixEditorFields.IntField(month);
            GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "M", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            EditorGUILayout.LabelField(".", GUILayout.Width(5));
            year = SirenixEditorFields.IntField(year);
            GUI.Label(GUILayoutUtility.GetLastRect().HorizontalPadding(0.0f, 8f), "Y", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            EditorGUILayout.EndHorizontal();

            month = Mathf.Clamp(month, 1, 12);
            var maxDays = DateTime.DaysInMonth(year, month);
            day = Mathf.Clamp(day, 1, maxDays);
            if (EditorGUI.EndChangeCheck())
            {
                try
                {
                    SetValue(new DateTime(year, month, day, hour, min, sec, milliSec));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            EditorGUILayout.BeginHorizontal();
            var dateTime = CurrentValue;
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            var firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var firstDayOfMonth = GetIndexForDayOfWeek(firstOfMonth.DayOfWeek);
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            {
                if (SirenixEditorGUI.IconButton(EditorIcons.TriangleLeft))
                {
                    if (month - 1 < 1) year -= 1;
                    month = (month - 2).PositiveModulo(12) + 1;
                    SetValue(new DateTime(year, month, day, hour, min, sec, milliSec));
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label($"{CurrentValue.ToString("MMMM", culture)} {year}");
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.IconButton(EditorIcons.TriangleRight))
                {
                    if (month + 1 > 12) year += 1;
                    month = month.PositiveModulo(12) + 1;
                    SetValue(new DateTime(year, month, day, hour, min, sec, milliSec));
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            foreach (var weekDay in WEEK_DAYS)
            {
                var names = culture.DateTimeFormat.AbbreviatedDayNames;
                var dayName = names[(int) weekDay];
                GUILayout.Label($"{dayName}", GUILayout.Width(29));
            }

            EditorGUILayout.EndHorizontal();

            var currentDay = 1;
            for (var w = 0; w < 6; w++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var d = 0; d < 7; d++)
                {
                    if ((w > 0 || d >= firstDayOfMonth) && currentDay <= daysInMonth)
                    {
                        var previousBgColor = GUI.backgroundColor;
                        if (currentDay == day)
                        {
                            GUI.backgroundColor = Color.cyan;
                        }
                        if (GUILayout.Button($"{currentDay}", GUILayout.Width(30)))
                        {
                            SetValue(new DateTime(year, month, currentDay, hour, min, sec, milliSec));
                        }
                        GUI.backgroundColor = previousBgColor;
                        currentDay++;
                    }
                    else
                    {
                        GUIHelper.PushGUIEnabled(false);
                        GUILayout.Button(string.Empty, GUILayout.Width(30), GUILayout.Height(18));
                        GUIHelper.PopGUIEnabled();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Epoch"))
                {
                    SetValue(DateTime.UnixEpoch);
                }
                if (GUILayout.Button("Now"))
                {
                    SetValue(DateTime.Now);
                }
                if (GUILayout.Button("Today"))
                {
                    SetValue(DateTime.Today);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
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
            EditorGUILayout.EndHorizontal();

            hour = Mathf.Clamp(hour, 0, 23);
            min = Mathf.Clamp(min, 0, 59);
            sec = Mathf.Clamp(sec, 0, 59);
            milliSec = Mathf.Clamp(milliSec, 0, 999);
            if (EditorGUI.EndChangeCheck())
            {
                try
                {
                    SetValue(new DateTime(year, month, day, hour, min, sec, milliSec));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        
        private static int GetIndexForDayOfWeek(DayOfWeek day)
        {
            return ((int) day - 1).PositiveModulo(7);
        }

        private void SetValue(DateTime value)
        {
            CurrentValue = value;
            OnValueChanged(value);
        }
    }
}
#endif