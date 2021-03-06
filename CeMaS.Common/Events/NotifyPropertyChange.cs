﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace CeMaS.Common.Events
{
    [DataContract]
    public abstract class NotifyPropertyChange :
        Disposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearEventHandlers();
            }
        }
        protected virtual void ClearEventHandlers()
        {
            PropertyChanged = null;
        }

        protected bool SetProperty<T>(Expression<Func<T>> property, ref T field, T value,
            Action<T, T> onValueChanged = null, EqualityComparer<T> valueComparer = null, bool onPropertyChange = true,
            Action<string, T> beforeChange = null)
        {
            if (valueComparer == null)
            {
                valueComparer = EqualityComparer<T>.Default;
            }
            var propertyInfo = property.PropertyInfo();
            if (beforeChange != null)
                beforeChange(propertyInfo.Name, value);
            T oldValue = (T)propertyInfo.GetValue(this);
            field = value;
            T newValue = (T)propertyInfo.GetValue(this);
            var changed = !valueComparer.Equals(oldValue, newValue);
            if (changed)
            {
                if (onValueChanged != null)
                    onValueChanged(oldValue, newValue);
                if (onPropertyChange)
                    OnPropertyChanged(propertyInfo.Name);
            }
            return changed;
        }
        protected bool SetProperty<T>(Expression<Func<T>> property, ref T field, T value,
            Action onValueChanged, EqualityComparer<T> valueComparer = null, bool onPropertyChange = true,
            Action<string, T> beforeChange = null)
        {
            return SetProperty(
                property, ref field, value,
                onValueChanged != null ? (o, n) => onValueChanged() : (Action<T, T>)null,
                valueComparer, onPropertyChange,
                beforeChange);
        }
        protected bool SetProperty<T>(Expression<Func<T>> property, Action<T> setValue, T value,
            Action<T, T> onValueChanged = null, EqualityComparer<T> valueComparer = null, bool onPropertyChange = true,
            Action<string, T> beforeChange = null)
        {
            if (valueComparer == null)
            {
                valueComparer = EqualityComparer<T>.Default;
            }
            var propertyInfo = property.PropertyInfo();
            if (beforeChange != null)
                beforeChange(propertyInfo.Name, value);
            T oldValue = (T)propertyInfo.GetValue(this);
            setValue(value);
            T newValue = (T)propertyInfo.GetValue(this);
            var changed = !valueComparer.Equals(oldValue, newValue);
            if (changed)
            {
                if (onValueChanged != null)
                    onValueChanged(oldValue, newValue);
                if (onPropertyChange)
                    OnPropertyChanged(propertyInfo.Name);
            }
            return changed;
        }
        protected bool SetProperty<T>(Expression<Func<T>> property, Action<T> setValue, T value,
            Action onValueChanged, EqualityComparer<T> valueComparer = null, bool onPropertyChange = true,
            Action<string, T> beforeChange = null)
        {
            return SetProperty(property, setValue, value,
                onValueChanged != null ? (o, n) => onValueChanged() : (Action<T, T>)null,
                valueComparer, onPropertyChange,
                beforeChange);
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            var propertyName = property.PropertyName();
            OnPropertyChanged(propertyName);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.RaiseEvent(this, propertyName);
        }
    }
}
