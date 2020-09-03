using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public class InProcessProtocol : IForwardingProtocol, IHandlerRegistrar {
        private readonly IDictionary<Uri, Func<IDocument, CancellationToken, Task>> _handlers =
            new Dictionary<Uri, Func<IDocument, CancellationToken, Task>>();

        public async Task ForwardAsync<TDocument>(Uri uri, TDocument document, CancellationToken token = default)
        where TDocument : IDocument {
            if (!_handlers.Keys.Contains(document.Routes.OnCompleted.Uri)) {
                throw new InvalidOperationException($"A handler has not been registered for `{document.Routes.OnCompleted.Uri}`.");
            }

            if (!_handlers.Keys.Contains(document.Routes.OnError.Uri)) {
                throw new InvalidOperationException($"A handler has not been registered for `{document.Routes.OnError.Uri}`.");
            }

            var notImplementedRoutes = document.Routes
                .Routes
                .Where(route => !_handlers.ContainsKey(route.Uri));

            if (notImplementedRoutes.Any()) {
                throw new AggregateException(
                    "One or more routes are not handled.  See innerExceptions for more information.",
                    notImplementedRoutes.Select(route => new InvalidOperationException($"A handler has not been registered for `{route.Uri}`.")));
            }

            // my expectation here was that any error would perform a rollback.
            // we may need to consider a property within the Routes object that can be set on RollBack to
            // instruct here to not execute a rollback and immediately forward to the OnError route.
            if (document.Routes.Direction == RoutingDirection.Reverse && document.Routes.ReversalCompleted()) {
                await _handlers[document.Routes.OnError.Uri].Invoke(document, token);
                return;
            }

            await _handlers[uri].Invoke(document, token);
            return;
        }

        public void Register<TDocument>(Uri uri, Func<TDocument, CancellationToken, Task> handler)
        where TDocument : IDocument {
            if (_handlers.ContainsKey(uri)) throw new Exception("Already registered!!!");

            _handlers.Add(uri, (doc, token) => handler((TDocument) doc, token));
        }

        public bool Handles(Uri uri) {
            return uri.Scheme.Equals("app", StringComparison.OrdinalIgnoreCase);
        }
    }
}