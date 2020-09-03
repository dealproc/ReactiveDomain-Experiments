using System;

using RD.Core.ValueObjects;

using ReactiveDomain.Messaging;

namespace RD.Core.Events.Project {
    public class ProjectRenamed : Event {
        public AccountId AccountId { get; set; }
        public ProjectId ProjectId { get; set; }
        public string OldProjectName { get; set; }
        public string NewProjectName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}