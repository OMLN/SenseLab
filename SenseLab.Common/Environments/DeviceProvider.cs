﻿using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public abstract class DeviceProvider<T> :
        EnvironmentNode<T>,
        IDeviceProvider
        where T : IDevice
    {
        public DeviceProvider(Guid id, string name, string description = null/*,
            INode parent = null*/)
            : base(id, name, description/*, parent*/)
        {
        }

        IEnumerable<IDevice> INode</*INode,*/ IDevice>.Children
        {
            get { return Children.Cast<IDevice>(); }
        }
    }
}
