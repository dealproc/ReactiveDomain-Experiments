using System;
using ReactiveDomain.Messaging;

namespace RD.Core.Tests {
    public class TestSource:ICorrelatedMessage {
        public Guid MsgId { get; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }

        public TestSource() {
            MsgId = Guid.NewGuid();
            CorrelationId = Guid.NewGuid();
            CausationId = Guid.NewGuid();
        }
    }
}