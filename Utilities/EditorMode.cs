// Author: František Holubec
// Copyright (c) UniLabs

using System;
using UnityEditor;

namespace UniLabs.Utilities
{
    [Flags]
    public enum EditorMode
    {
        EditMode = 1 << 0,
        PlayMode = 1 << 1,
        Both = EditMode | PlayMode
    }

    public static class EditorModeExtensions
    {
        public static bool HasFlagFast(this EditorMode value, EditorMode flag)
        {
            return (value & flag) != 0;
        }

#if UNITY_EDITOR
        public static bool IsCurrentEditorMode(this EditorMode value)
        {
            return value.HasFlagFast(EditorApplication.isPlaying ? EditorMode.PlayMode : EditorMode.EditMode);
        }
#endif
    }
}
