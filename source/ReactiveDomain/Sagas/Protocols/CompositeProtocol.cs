using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public class CompositeProtocol : IForwardingProtocol, Handles<IDocument> {
        private readonly IReadOnlyList<IForwardingProtocol> _protocols;

        public CompositeProtocol(params IForwardingProtocol[] protocols) =>
            _protocols = new List<IForwardingProtocol>(protocols);

        public async Task HandleAsync(IDocument document, CancellationToken token = default) {
            var slip = document.Routes.GetRoute();
            if (slip != null) await ForwardAsync(slip.Uri, document, token);
        }

        public async Task ForwardAsync<TDocument>(Uri uri, TDocument document, CancellationToken token = default)
            where TDocument : IDocument {
            var tasks = _protocols.Select(p => p.ForwardAsync(uri, document, token)).ToArray();

            if (!tasks.Any()) throw new InvalidOperationException($"No forwarding protocols exist for {uri}");

            await Task.WhenAll(tasks);
        }
    }
}