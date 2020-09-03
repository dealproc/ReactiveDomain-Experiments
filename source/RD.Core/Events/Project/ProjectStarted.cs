using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Events.Project {
    public class ProjectStarted : Event {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public DateTime Timestamp { get; set; }
    }
}