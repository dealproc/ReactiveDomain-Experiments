using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveDomain.Sagas {
    public class RoutingTable {
        private readonly List<RoutingSlip> _routes;

        /// <summary>
        ///     Builds an instance of Saga Routes.
        /// </summary>
        /// <param name="routes">The list of routes for the assigned saga.</param>
        /// <param name="onCompleted">The route to call when the saga has completed.</param>
        /// <param name="onError">The route to call if an error has occurred.</param>
        public RoutingTable(IEnumerable<RoutingSlip> routes, RoutingSlip onCompleted, RoutingSlip onError) {
            if (!routes?.Any() ?? true) throw new ArgumentException("At least one route is necessary.", nameof(routes));

            OnCompleted = onCompleted ??
                          throw new ArgumentException("A completion route is required.", nameof(onCompleted));
            OnError = onError ?? throw new ArgumentException("An error route is required.", nameof(onError));
            OnError = onError;


            _routes = new List<RoutingSlip>(routes);
        }

        public IReadOnlyList<RoutingSlip> Routes => _routes;
        public RoutingDirection Direction { get; private set; } = RoutingDirection.Forward;

        public RoutingSlip OnCompleted { get; }
        public RoutingSlip OnError { get; }

        /// <summary>
        ///     Resolves the current, active route.
        /// </summary>
        /// <returns>The current, active route.</returns>
        public RoutingSlip GetRoute() =>
            Direction == RoutingDirection.Forward
                ? Routes.FirstOrDefault(route => !route.HasCompleted) ?? OnCompleted
                : Routes.Reverse().FirstOrDefault(route => route.HasCompleted) ?? OnError;

        /// <summary>
        ///     Marks the current, active route as completed.
        /// </summary>
        public void FinishRoute() {
            switch (Direction) {
                case RoutingDirection.Forward:
                    var activeRoute = GetRoute();
                    if (activeRoute != null) {
                        activeRoute.HasCompleted = true;
                        activeRoute.CompletionDate = DateTime.UtcNow;
                    }

                    break;
                case RoutingDirection.Reverse:
                    var priorRoute = Routes.Reverse().FirstOrDefault(route => route.HasCompleted);
                    if (priorRoute != null) {
                        priorRoute.HasCompleted = false;
                        priorRoute.CompletionDate = DateTime.MinValue;
                    }

                    break;
            }
        }

        public bool HasCompleted() => _routes.All(route => route.HasCompleted);

        public bool ReversalCompleted() =>
            Routes.All(route => !route.HasCompleted) && Direction == RoutingDirection.Reverse;

        public void RollBack() => Direction = RoutingDirection.Reverse;
    }

    public enum RoutingDirection {
        Forward,
        Reverse
    }
}