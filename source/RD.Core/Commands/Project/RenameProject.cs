using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Commands.Project {
    public class RenameProject : Command {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}