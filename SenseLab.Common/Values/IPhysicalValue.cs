﻿using CeMaS.Common.PhysicalQuantities;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValue<T> :
        IValue<T>
    {
        PhysicalQuantity Quantity { get; }
        Unit Unit { get; }
        IPrecision Precision { get; }
    }
}
