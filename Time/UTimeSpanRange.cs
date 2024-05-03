// Author: František Holubec
// Copyright (c) UniLabs

using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UniLabs.Time
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    [InlineProperty]
    public class UTimeSpanRange
    {
        [JsonProperty("Start")]
        [SerializeField]
        [LabelWidth(22)]
        [HideLabel]
        [OnValueChanged(nameof(OnStartChanged))]
        private UTimeSpan _Start;

        [JsonProperty("End")]
        [SerializeField]
        [LabelWidth(22)]
        [HideLabel]
        [OnValueChanged(nameof(OnEndChanged))]
        private UTimeSpan _End;

        public TimeSpan Start
        {
            get => _Start;
            set => _Start = value;
        }

        public TimeSpan End
        {
            get => _End;
            set => _End = value;
        }

        public TimeSpan Duration => End - Start;

        public bool IsInRange(TimeSpan time)
        {
            return time >= Start && time <= End;
        }

        [JsonConstructor]
        public UTimeSpanRange() { }

        public UTimeSpanRange(TimeSpan start)
        {
            _Start = start;
            _End = start;
        }

        public UTimeSpanRange(TimeSpan start, TimeSpan end)
        {
            _Start = start;
            _End = end;
        }

        private void OnStartChanged()
        {
            if (_Start.CompareTo(_End) > 0) _End.TimeSpan = _Start.TimeSpan;
        }

        private void OnEndChanged()
        {
           if (_End.CompareTo(_Start) < 0) _Start.TimeSpan = _End.TimeSpan;
        }
    }
}
