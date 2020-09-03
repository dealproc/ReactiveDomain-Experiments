using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public interface IHandlerRegistrar {
        void Register<TDocument>(Uri uri, Func<TDocument, CancellationToken, Task> handler) where TDocument : IDocument;
    }
}