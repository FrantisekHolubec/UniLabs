// Author: František Holubec
// Copyright (c) UniLabs

#if UNITY_EDITOR
using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace UniLabs.Time
{
    [UsedImplicitly]
    public sealed class UDateTimeDrawer : OdinValueDrawer<UDateTime>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            TimeDrawerUtils.DrawDateTimeField(label, ValueEntry.SmartValue, value =>
            {
                ValueEntry.SmartValue = value;
                ValueEntry.ApplyChanges();
                Property.MarkSerializationRootDirty();
            });
        }
    }

    [UsedImplicitly]
    public sealed class DateTimeDrawer : OdinValueDrawer<DateTime>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            TimeDrawerUtils.DrawDateTimeField(label, ValueEntry.SmartValue, value =>
            {
                ValueEntry.SmartValue = value;
                ValueEntry.ApplyChanges();
                Property.MarkSerializationRootDirty();
            });
        }
    }
}
#endif
