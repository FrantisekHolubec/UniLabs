// Author: František Holubec
// Created: 03.05.2024
// Copyright (c) Noxgames
// http://www.noxgames.com/

#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UniLabs.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UniLabs.Time
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public sealed class UTimeSpanRangeAttributeDrawer : OdinAttributeDrawer<TimeSpanRangeAttribute, UTimeSpan>
    {
        private TimeSpanRangeHelper _helper;

        protected override void Initialize()
        {
            _helper = new TimeSpanRangeHelper(Attribute, Property);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            _helper.Draw(label, ValueEntry.SmartValue, CallNextDrawer, value =>
            {
                ValueEntry.SmartValue = value;
            });
        }
    }

    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public sealed class TimeSpanRangeAttributeDrawer : OdinAttributeDrawer<TimeSpanRangeAttribute, TimeSpan>
    {
        private TimeSpanRangeHelper _helper;

        protected override void Initialize()
        {
            _helper = new TimeSpanRangeHelper(Attribute, Property);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            _helper.Draw(label, ValueEntry.SmartValue, CallNextDrawer, value =>
            {
                ValueEntry.SmartValue = value;
            });
        }
    }

    public class TimeSpanRangeHelper
    {
        private readonly ValueResolver<TimeSpan> _minValueResolver;
        private readonly ValueResolver<TimeSpan> _maxValueResolver;

        private readonly TimeSpanRangeAttribute _attribute;

        public TimeSpanRangeHelper(TimeSpanRangeAttribute attribute, InspectorProperty property)
        {
            _attribute = attribute;
            if (attribute.MinGetter != null) _minValueResolver = ValueResolver.Get<TimeSpan>(property, attribute.MinGetter);
            if (attribute.MaxGetter != null) _maxValueResolver = ValueResolver.Get<TimeSpan>(property, attribute.MaxGetter);
        }

        public void Draw(GUIContent label, TimeSpan value, Func<GUIContent, bool> callNextDrawer, Action<TimeSpan> onValueChanged)
        {
            ValueResolver.DrawErrors(_minValueResolver, _maxValueResolver);
            if (_attribute.Inline)
            {
                SirenixEditorGUI.BeginHorizontalPropertyLayout(label);
            }
            else
            {
                SirenixEditorGUI.BeginVerticalPropertyLayout(label);
                callNextDrawer(null);
            }

            EditorGUI.BeginChangeCheck();
            var min = _minValueResolver?.GetValue() ?? TimeSpan.Zero;
            var max = _maxValueResolver?.GetValue() ?? TimeSpan.Zero;

            var newValue = TimeDrawerUtils.DrawTimeSpanSlider(value, min, max, _attribute.SnappingUnit);
            if (EditorGUI.EndChangeCheck())
            {
                onValueChanged?.Invoke(newValue);
            }

            if (_attribute.Inline)
            {
                callNextDrawer(null);
                GUILayout.Space(1);
                SirenixEditorGUI.EndHorizontalPropertyLayout();
            }
            else
            {
                SirenixEditorGUI.EndVerticalPropertyLayout();
            }
        }

        [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
        public sealed class TimeSpanRangeMinMaxSliderAttributeDrawer : OdinAttributeDrawer<TimeSpanRangeAttribute, UTimeSpanRange>
        {
            private ValueResolver<TimeSpan> _minValueResolver;
            private ValueResolver<TimeSpan> _maxValueResolver;
            private ValueResolver<bool> _disableMinMaxIfResolver;

            protected override void Initialize()
            {
                if (Attribute.MinGetter != null) _minValueResolver = ValueResolver.Get<TimeSpan>(Property, Attribute.MinGetter);
                if (Attribute.MaxGetter != null) _maxValueResolver = ValueResolver.Get<TimeSpan>(Property, Attribute.MaxGetter);
                if (Attribute.DisableMinMaxIf != null) _disableMinMaxIfResolver = ValueResolver.Get<bool>(Property, Attribute.DisableMinMaxIf);
            }

            protected override void DrawPropertyLayout(GUIContent label)
            {
                ValueResolver.DrawErrors(_minValueResolver, _maxValueResolver, _disableMinMaxIfResolver);
                var min = _minValueResolver?.GetValue() ?? TimeSpan.Zero;
                var max = _maxValueResolver?.GetValue() ?? TimeSpan.Zero;
                var childTimeSpans = Property.Children.Where(c => c.ValueEntry.BaseValueType == typeof(UTimeSpan)).ToList();
                var startProperty = childTimeSpans[0];
                var endProperty = childTimeSpans[1];
                var disableMinMax = _disableMinMaxIfResolver?.GetValue() ?? false;

                if (Attribute.Inline)
                {
                    SirenixEditorGUI.BeginHorizontalPropertyLayout(label);
                    EditorGUI.BeginChangeCheck();
                    startProperty.Draw(GUIContent.none);
                    if (EditorGUI.EndChangeCheck()) startProperty.ForceMarkDirty();
                    GUILayout.Space(1);
                }
                else
                {
                    SirenixEditorGUI.BeginVerticalPropertyLayout(label);
                    if (!disableMinMax)
                    {
                        CallNextDrawer(null);
                    }
                    else
                    {
                        startProperty.Draw(GUIContent.none);
                    }
                }
                EditorGUI.BeginChangeCheck();
                if (!disableMinMax)
                {
                    var (newStart, newEnd) = TimeDrawerUtils.DrawTimeSpanMinMaxSlider(ValueEntry.SmartValue.Start, ValueEntry.SmartValue.End, min, max, Attribute.SnappingUnit);
                    if (EditorGUI.EndChangeCheck())
                    {
                        ValueEntry.SmartValue.Start = newStart;
                        ValueEntry.SmartValue.End = newEnd;
                        Property.ForceMarkDirty();
                    }
                }
                else
                {
                    var newStart = TimeDrawerUtils.DrawTimeSpanSlider(ValueEntry.SmartValue.Start, min, max, Attribute.SnappingUnit);
                    if (EditorGUI.EndChangeCheck())
                    {
                        ValueEntry.SmartValue.Start = newStart;
                        Property.ForceMarkDirty();
                    }
                }
                if (Attribute.Inline)
                {
                    if (!disableMinMax)
                    {
                        GUILayout.Space(1);
                        EditorGUI.BeginChangeCheck();
                        endProperty.Draw(GUIContent.none);
                        if (EditorGUI.EndChangeCheck()) endProperty.ForceMarkDirty();
                    }
                    else
                    {
                        GUILayout.Space(1);
                        GUIHelper.PushGUIEnabled(false);
                        startProperty.Draw(GUIContent.none);
                        GUIHelper.PopGUIEnabled();
                    }
                    SirenixEditorGUI.EndVerticalPropertyLayout();
                }
                else
                {
                    SirenixEditorGUI.EndVerticalPropertyLayout();
                }
            }
        }
    }
}
#endif
