// Author: František Holubec
// Copyright (c) UniLabs

using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace UniLabs.Time
{
    /// <summary>
    /// Unity serializable wrapper for TimeSpan, usually necessary for fields in Monobehaviour or ScriptableObject
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UTimeSpan : ISerializationCallbackReceiver, IComparable<UTimeSpan>, IComparable<TimeSpan>
    {
        [JsonProperty("TimeSpan")]
        public TimeSpan TimeSpan { get; set; }

        [HideInInspector]
        [SerializeField]
        private string _TimeSpan;

        [JsonConstructor]
        public UTimeSpan()
        {
            TimeSpan = TimeSpan.Zero;
        }

        public UTimeSpan(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        public UTimeSpan(long ticks) : this(new TimeSpan(ticks)) {}
        public UTimeSpan(int hours, int minutes, int seconds) : this(new TimeSpan(hours, minutes, seconds)) {}
        public UTimeSpan(int days, int hours, int minutes, int seconds) : this(new TimeSpan(days, hours, minutes, seconds)) {}
        public UTimeSpan(int days, int hours, int minutes, int seconds, int milliseconds) : this(new TimeSpan(days, hours, minutes, seconds, milliseconds)) {}

        public static implicit operator TimeSpan(UTimeSpan uTimeSpan) => uTimeSpan?.TimeSpan ?? TimeSpan.Zero;
        public static implicit operator UTimeSpan(TimeSpan timeSpan) => new(timeSpan);

        public int CompareTo(TimeSpan other)
        {
            return TimeSpan.CompareTo(other);
        }

        public int CompareTo(UTimeSpan other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return TimeSpan.CompareTo(other.TimeSpan);
        }

        protected bool Equals(UTimeSpan other)
        {
            return TimeSpan.Equals(other.TimeSpan);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UTimeSpan) obj);
        }

        public override int GetHashCode()
        {
            return TimeSpan.GetHashCode();
        }

        public void OnAfterDeserialize()
        {
            TimeSpan = TimeSpan.TryParse(_TimeSpan, CultureInfo.InvariantCulture, out var result) ? result : TimeSpan.Zero;
        }

        public void OnBeforeSerialize()
        {
            _TimeSpan = TimeSpan.ToString();
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            OnBeforeSerialize();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            OnAfterDeserialize();
        }
    }
}
