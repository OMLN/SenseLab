﻿using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class TimeInterval :
        TemporalLocation,
        ITimeInterval
    {
        public TimeInterval(DateTimeOffset from, TimeSpan? length = null)
        {
            From = from;
            if (length.HasValue)
                length = TimeSpan.Zero;
            Length = length.Value;
        }
        public TimeInterval(DateTimeOffset from, DateTimeOffset to)
        {
            From = from;
            To = to;
        }

        public static TimeInterval Now { get { return new TimeInterval(DateTimeOffset.Now); } }

        public DateTimeOffset From
        {
            get { return from; }
            set
            {
                SetProperty(() => From, ref from, value, OnFromChanged);
            }
        }
        public TimeSpan Length
        {
            get { return length; }
            set
            {
                SetProperty(() => Length, ref length, value, () => OnLengthChanged(true));
            }
        }
        public DateTimeOffset To
        {
            get { return From + Length; }
            set
            {
                SetProperty(() => To, v => Length = From - To, value);
            }
        }

        public new ITimeInterval Clone()
        {
            return (ITimeInterval)base.Clone();
        }
        public override string ToString()
        {
            return Length == TimeSpan.Zero ?
                From.ToString() :
                string.Format("{0} - {1} ({2})", From, To, Length);
        }

        protected virtual void OnFromChanged()
        {
            OnPropertyChanged(() => From);
            OnLengthChanged(false);
            OnChanged();
        }
        protected virtual void OnLengthChanged(bool onToChanged)
        {
            OnPropertyChanged(() => Length);
            if (onToChanged)
            {
                OnToChanged();
                OnChanged();
            }
        }
        protected virtual void OnToChanged()
        {
            OnPropertyChanged(() => To);
            OnChanged();
        }

        [DataMember(Name = "From")]
        private DateTimeOffset from;
        [DataMember(Name = "Length")]
        private TimeSpan length;
    }
}
