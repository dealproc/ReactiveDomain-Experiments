using System;
using System.Collections.Generic;
using System.Text;
using RD.Core.ValueObjects;
using ReactiveDomain.Messaging;

namespace RD.Core.Messages
{
    public class DeviceMsgs
    {
        public class Activate : Command {
            public readonly Guid DeviceId;
            public readonly Guid MyId;
            public readonly DateTime Timestamp;

            public Activate(Guid deviceId,  Guid myId, DateTime timestamp) {
                DeviceId = deviceId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Activated : Event {
            public readonly Guid DeviceId;
            public readonly Guid MyId;
            public readonly DateTime Timestamp;

            public Activated(Guid deviceId,  Guid myId, DateTime timestamp) {
                DeviceId = deviceId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Deactivate : Command {
            public readonly Guid DeviceId;
            public readonly Guid MyId;
            public readonly DateTime Timestamp;

            public Deactivate(Guid deviceId,  Guid myId, DateTime timestamp) {
                DeviceId = deviceId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Deactivated : Event {
            public readonly Guid DeviceId;
            public readonly Guid MyId;
            public readonly DateTime Timestamp;

            public Deactivated(Guid deviceId,  Guid myId, DateTime timestamp) {
                DeviceId = deviceId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Provision : Command {
            public readonly Guid DeviceId; 
            public readonly Guid AccountId;
            public readonly Guid MyId; 
            public readonly string TID;
            public readonly string Description; 
            public readonly DateTime Timestamp;
            public Provision(Guid deviceId, Guid accountId, Guid myId, string tid, string description, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                TID = tid;
                Description = description;
                Timestamp = timestamp;
            }
        }
        public class Provisioned : Event {
            public readonly Guid DeviceId; 
            public readonly Guid AccountId;
            public readonly Guid MyId; 
            public readonly string TID;
            public readonly string Description; 
            public readonly DateTime Timestamp;
            public Provisioned(Guid deviceId, Guid accountId, Guid myId, string tid, string description, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                TID = tid;
                Description = description;
                Timestamp = timestamp;
            }
        }
    }
}
