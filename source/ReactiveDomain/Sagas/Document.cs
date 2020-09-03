using System;

namespace ReactiveDomain.Sagas {
    public interface IDocument {
        RoutingTable Routes { get; }
        Exception Exception { get; }

        void SetException(Exception exc);
    }

    public abstract class Document : IDocument {
        public Document(RoutingTable routing) {
            Routes = routing;
        }

        public RoutingTable Routes { get; }
        public Exception Exception { get; private set; }

        public void SetException(Exception exc) {
            Exception = exc;
            Routes.RollBack();
        }
    }
}