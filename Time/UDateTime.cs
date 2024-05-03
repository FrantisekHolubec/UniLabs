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
    /// Unity serializable wrapper for DateTime, usually necessary for fields in MonoBehaviour or ScriptableObject
    /// Based on https://gist.github.com/EntranceJew/f329f1c6a0c35ac51763455f76b5eb95
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UDateTime : ISerializationCallbackReceiver, IComparable<UDateTime>, IComparable<DateTime>
    {
        [JsonProperty("DateTime")]
        public DateTime DateTime { get; set; }

        [HideInInspector]
        [SerializeField]
        private string _DateTime;

        [JsonConstructor]
        public UDateTime()
        {
            DateTime = DateTime.UnixEpoch;
        }

        public UDateTime(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public static implicit operator DateTime(UDateTime udt) => udt.DateTime;
        public static implicit operator UDateTime(DateTime dt) => new() {DateTime = dt};

        public int CompareTo(DateTime other)
        {
            return DateTime.CompareTo(other);
        }

        public int CompareTo(UDateTime other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return DateTime.CompareTo(other.DateTime);
        }

        protected bool Equals(UDateTime other)
        {
            return DateTime.Equals(other.DateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UDateTime) obj);
        }

        public override int GetHashCode()
        {
            return DateTime.GetHashCode();
        }

        public override string ToString()
        {
            return DateTime.ToString(CultureInfo.InvariantCulture);
        }

        public void OnAfterDeserialize()
        {
            DateTime = DateTime.TryParse(_DateTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result) ? result : DateTime.MinValue;
        }

        public void OnBeforeSerialize()
        {
            _DateTime = DateTime.ToString("o", CultureInfo.InvariantCulture);
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            OnBeforeSerialize();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            OnAfterDeserialize();
        }
    }
}
