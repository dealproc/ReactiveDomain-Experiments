using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Events.Project {
    public class SecretHasBeenReset : Event {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public string OldSecret { get; set; }
        public string NewSecret { get; set; }
        public DateTime Timestamp { get; set; }
    }
}