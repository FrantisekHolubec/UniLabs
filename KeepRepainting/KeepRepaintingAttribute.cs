// Author: František Holubec
// Copyright (c) UniLabs

using System;
using System.Diagnostics;
using UniLabs.Utilities;

namespace UniLabs.KeepRepainting
{
    [Conditional("UNITY_EDITOR")]
    public class KeepRepaintingAttribute : Attribute
    {
        // Can be set to 0 to refresh every frame
        public float RepaintInterval = 0.5f;

        public EditorMode RepaintEditorMode = EditorMode.Both;

        public string RepaintIf;

        public KeepRepaintingAttribute() { }
        public KeepRepaintingAttribute(EditorMode repaintEditorMode, float repaintInterval = 0.5f)
        {
            RepaintEditorMode = repaintEditorMode;
            RepaintInterval = repaintInterval;
        }
    }
}
