using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public interface IForwardingProtocol {
        Task ForwardAsync<TDocument>(Uri uri, TDocument document, CancellationToken token = default)
            where TDocument : IDocument;
    }
}