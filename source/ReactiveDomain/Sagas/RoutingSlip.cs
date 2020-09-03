using System;

namespace ReactiveDomain.Sagas {
    public class RoutingSlip {
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public bool HasCompleted { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}