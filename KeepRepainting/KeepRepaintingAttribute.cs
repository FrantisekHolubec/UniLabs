// Author: František Holubec
// Copyright (c) UniLabs

using System;
using System.Diagnostics;
using UniLabs.Utilities.Editor;

namespace UniLabs.KeepRepainting
{
    [Conditional("UNITY_EDITOR")]
    public class KeepRepaintingAttribute : Attribute
    {
        // Can be set to 0 to refresh every frame
        public float RefreshFrequency = 0.5f;

        public EditorMode RepaintEditorMode = EditorMode.Both;

        public string RepaintIf;

        public KeepRepaintingAttribute() { }
        public KeepRepaintingAttribute(EditorMode repaintEditorMode, float refreshFrequency = 0.5f)
        {
            RepaintEditorMode = repaintEditorMode;
            RefreshFrequency = refreshFrequency;
        }
    }
}
