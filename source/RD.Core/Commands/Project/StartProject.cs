using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Commands.Project {
    public class StartProject : Command {
        public ProjectId ProjectId { get; set; }
        public AccountId AccountId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}