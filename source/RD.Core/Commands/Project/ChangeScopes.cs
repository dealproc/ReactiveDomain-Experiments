using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Commands.Project {
    public class ChangeScopes : Command {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public Scopes Scopes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}