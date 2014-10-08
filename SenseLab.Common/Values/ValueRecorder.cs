﻿using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;

namespace SenseLab.Common.Values
{
    public class ValueRecorder<T> :
        SamplingRecorder<ValueRecord<T>>
    {
        public ValueRecorder(IValue<T> value, uint nextSequenceNumber, ILocatable<ISpatialLocation> location)
            : base(nextSequenceNumber, location)
        {
            value.ValidateNonNull("value");
            Value = value;
        }

        public IValue<T> Value { get; private set; }
        public override IRecordable Recordable
        {
            get { return Value; }
        }

        protected override void DoStart()
        {
            AddRecord();
            Value.ValueChanged += OnValueChanged;
        }
        protected override void DoStop()
        {
            Value.ValueChanged -= OnValueChanged;
        }
        protected override void DoPause()
        {
            DoStop();
        }
        protected override void DoUnpause()
        {
            DoStart();
        }
        protected override ValueRecord<T> CreateRecord(object data, uint sequenceNumber, ISpatialLocation spatialLocation)
        {
            var a = (ValueChangeEventArgs<T>)data;
            T value;
            ITime temporalLocation = null;
            if (a != null)
            {
                value = a.NewValue;
                temporalLocation = a.Location as ITime;
            }
            else
            {
                Value.ReadValue();
                value = Value.Value;
            }
            if (temporalLocation == null)
                temporalLocation = Time.Now;
            return new ValueRecord<T>(
                Recordable, sequenceNumber, temporalLocation,
                value,
                spatialLocation);
        }

        private void OnValueChanged(object sender, ValueChangeEventArgs<T> e)
        {
            AddRecord(e);
        }
    }
}
