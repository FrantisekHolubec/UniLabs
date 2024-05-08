// Author: František Holubec
// Copyright (c) UniLabs

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UniLabs.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UniLabs.KeepRepainting
{
    public static class KeepRepaintingUtility
    {
        private static Dictionary<PropertyTree, EditorRecord> _treeRepaintTimes = new Dictionary<PropertyTree, EditorRecord>();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            EditorApplication.playModeStateChanged -= OnPlayModeSateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeSateChanged;
        }

        private static void OnPlayModeSateChanged(PlayModeStateChange changeType)
        {
            foreach (var tree in _treeRepaintTimes.Keys)
            {
                _treeRepaintTimes[tree].LastRepaint = 0;
            }
        }

        public static void DrawerRepainted(KeepRepaintingAttributeDrawer drawer)
        {
            _treeRepaintTimes[drawer.Property.Tree].LastRepaint = UnityEngine.Time.realtimeSinceStartup;
        }

        public static void RegisterDrawer(KeepRepaintingAttributeDrawer drawer)
        {
            var property = drawer.Property;
            if (_treeRepaintTimes.TryGetValue(property.Tree, out var record))
            {
                record.Drawers.Add(drawer);
            }
            else
            {
                if (TryGetEditor(property.Tree, out var editor))
                    _treeRepaintTimes.Add(property.Tree, new EditorRecord(drawer, editor));
            }
        }

        public static void UnregisterDrawer(KeepRepaintingAttributeDrawer drawer)
        {
            var property = drawer.Property;
            if (!_treeRepaintTimes.TryGetValue(property.Tree, out var record))
                return;

            record.Drawers.Remove(drawer);
            if (record.Drawers.Count == 0)
                _treeRepaintTimes.Remove(property.Tree);
        }

        private static bool TryGetEditor(PropertyTree tree, out OdinEditor result)
        {
            foreach (var editor in Resources.FindObjectsOfTypeAll<OdinEditor>())
            {
                if (editor.Tree != tree)
                    continue;

                result = editor;
                return true;
            }

            result = null;
            return false;
        }

        private static void OnEditorUpdate()
        {
            if (_treeRepaintTimes == null || _treeRepaintTimes.Count == 0)
                return;

            _treeRepaintTimes.Where(x => x.Value?.Editor == null)
                .ToList()
                .ForEach(pair => _treeRepaintTimes.Remove(pair.Key));

            foreach (var editorRecords in _treeRepaintTimes.Values)
            {
                editorRecords?.TryRepaint();
            }
        }

        private class EditorRecord
        {
            public List<KeepRepaintingAttributeDrawer> Drawers = new List<KeepRepaintingAttributeDrawer>();
            public float LastRepaint;
            public readonly OdinEditor Editor;

            public bool TryRepaint()
            {
                if (!CanRepaint())
                    return false;

                Editor.Repaint();
                LastRepaint = UnityEngine.Time.realtimeSinceStartup;
                return true;
            }

            private bool CanRepaint()
            {
                foreach (var drawer in Drawers)
                {
                    if (drawer.Property.State.Visible &&
                        UnityEngine.Time.realtimeSinceStartup - LastRepaint >= drawer.Attribute.RefreshFrequency &&
                        drawer.Attribute.RepaintEditorMode.IsCurrentEditorMode() &&
                        drawer.RepaintIfResolver.GetValue())
                    {
                        return true;
                    }
                }
                return false;
            }

            public EditorRecord(KeepRepaintingAttributeDrawer drawer, OdinEditor editor, float lastRepaint = 0)
            {
                Drawers.Add(drawer);
                LastRepaint = lastRepaint;
                Editor = editor;
            }
        }
    }
}
#endif
