using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Events.Device {
    public class Provisioned : Event {
        public DeviceId Id { get; set; }
        public AccountId AccountId { get; set; }
        public MyId MyId { get; set; }
        public string TID { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}