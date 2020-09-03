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
            public readonly DeviceId DeviceId;
            public readonly AccountId AccountId;
            public readonly MyId MyId;
            public readonly DateTime Timestamp;

            public Activate(DeviceId deviceId, AccountId accountId, MyId myId, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Activated : Event {
            public readonly DeviceId DeviceId;
            public readonly AccountId AccountId;
            public readonly MyId MyId;
            public readonly DateTime Timestamp;

            public Activated(DeviceId deviceId, AccountId accountId, MyId myId, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Deactivate : Command {
            public readonly DeviceId DeviceId;
            public readonly AccountId AccountId;
            public readonly MyId MyId;
            public readonly DateTime Timestamp;

            public Deactivate(DeviceId deviceId, AccountId accountId, MyId myId, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Deactivated : Event {
            public readonly DeviceId DeviceId;
            public readonly AccountId AccountId;
            public readonly MyId MyId;
            public readonly DateTime Timestamp;

            public Deactivated(DeviceId deviceId, AccountId accountId, MyId myId, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                Timestamp = timestamp;
            }
        }
        public class Provision : Command {
            public readonly DeviceId DeviceId; 
            public readonly AccountId AccountId;
            public readonly MyId MyId; 
            public readonly string TID;
            public readonly string Description; 
            public readonly DateTime Timestamp;
            public Provision(DeviceId deviceId, AccountId accountId, MyId myId, string tid, string description, DateTime timestamp) {
                DeviceId = deviceId;
                AccountId = accountId;
                MyId = myId;
                TID = tid;
                Description = description;
                Timestamp = timestamp;
            }
        }
        public class Provisioned : Event {
            public readonly DeviceId DeviceId; 
            public readonly AccountId AccountId;
            public readonly MyId MyId; 
            public readonly string TID;
            public readonly string Description; 
            public readonly DateTime Timestamp;
            public Provisioned(DeviceId deviceId, AccountId accountId, MyId myId, string tid, string description, DateTime timestamp) {
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
