// Author: František Holubec
// Copyright (c) UniLabs

#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;

namespace UniLabs.Utilities.Editor
{
    public static class InspectorPropertyExtensions
    {
        public static bool HasParentObject<T>(this InspectorProperty property, Predicate<T> filter = null, bool includeSelf = false)
        {
            return TryGetParentObject<T>(property, out _, out _, filter, includeSelf);
        }

        public static T GetParentObject<T>(this InspectorProperty property, Predicate<T> filter = null, bool includeSelf = false)
        {
            TryGetParentObject(property, out var result, out _, filter, includeSelf);
            return result;
        }

        public static bool TryGetParentObject<T>(this InspectorProperty property, out T result, Predicate<T> filter = null, bool includeSelf = false)
        {
            return TryGetParentObject(property, out result, out _, filter, includeSelf);
        }

        public static bool TryGetParentObject<T>(this InspectorProperty property, out T result, out InspectorProperty parentProperty, Predicate<T> filter = null, bool includeSelf = false)
        {
            Func<InspectorProperty, bool> predicate = filter != null ?
                p => p?.ValueEntry?.WeakSmartValue is T tVal && filter(tVal) :
                p => p?.ValueEntry?.WeakSmartValue is T;

            parentProperty = property.FindParent(predicate, includeSelf);
            if (parentProperty?.ValueEntry.WeakSmartValue is T tResult)
            {
                result = tResult;
                return true;
            }
            result = default;
            return false;
        }

        public static void ForceMarkDirty(this InspectorProperty property, bool alsoMarkRootDirty = true)
        {
            if (property?.ValueEntry?.WeakValues == null)
                return;

            property.ValueEntry.WeakValues.ForceMarkDirty();
            property.ValueEntry.ApplyChanges();
            if (alsoMarkRootDirty) property.MarkSerializationRootDirty();
        }
    }
}
#endif
