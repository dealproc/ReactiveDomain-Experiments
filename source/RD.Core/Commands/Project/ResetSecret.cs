using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Commands.Project {
    public class ResetSecret : Command {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}