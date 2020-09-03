using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Commands.Device {
    public class Provision : Command {
        public DeviceId Id { get; set; }
        public AccountId AccountId { get; set; }
        public MyId MyId { get; set; }
        public long DeviceIdOffset { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}