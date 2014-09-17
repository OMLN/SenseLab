﻿using SenseLab.Common.Events;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Common.Values
{
    public abstract class Value<T> :
        Recordable<ValueRecorder<T>>,
        IValue<T>
    {
        public Value()
        {

        }

        public bool HasValue
        {
            get { throw new NotImplementedException(); }
        }
        T IValue<T>.Value
        {
            get { throw new NotImplementedException(); }
        }
        public event EventHandler<ValueChangeEventArgs<T>> ValueChanged;

        public bool ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        public T ValueWritable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool WriteValue()
        {
            throw new NotImplementedException();
        }
    }
}