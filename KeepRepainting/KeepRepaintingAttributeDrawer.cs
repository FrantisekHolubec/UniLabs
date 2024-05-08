// Author: František Holubec
// Created: 08.05.2024
// Copyright (c) Noxgames
// http://www.noxgames.com/

#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UnityEngine;

namespace UniLabs.KeepRepainting
{
    [DrawerPriority(1000)]
    public sealed class KeepRepaintingAttributeDrawer : OdinAttributeDrawer<KeepRepaintingAttribute>, IDisposable
    {
        public ValueResolver<bool> RepaintIfResolver { get; private set; }

        protected override void Initialize()
        {
            RepaintIfResolver = ValueResolver.Get(Property, Attribute.RepaintIf, true);

            base.Initialize();
            KeepRepaintingUtility.RegisterDrawer(this);
        }

        public void Dispose()
        {
            KeepRepaintingUtility.UnregisterDrawer(this);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueResolver.DrawErrors(RepaintIfResolver);
            CallNextDrawer(label);
            KeepRepaintingUtility.DrawerRepainted(this);
        }
    }
}
#endif
