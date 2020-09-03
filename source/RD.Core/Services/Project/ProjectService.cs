using System;

using RD.Core.Commands.Project;

using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.Services.Project {
    public class ProjectService : IHandle<NewProjectRequest>, IHandle<Commands.Project.ChangeScopes>, IHandle<Commands.Project.CloseProject>, IHandle<Commands.Project.RenameProject>, IHandle<Commands.Project.ReopenProject>, IHandle<Commands.Project.ResetSecret> {
        private readonly IDispatcher _dispatcher;
        private readonly ICorrelatedRepository _repository;
        public ProjectService(IDispatcher dispatcher, ICorrelatedRepository repository) {
            _dispatcher = dispatcher;
            _repository = repository;

            _dispatcher.Subscribe<NewProjectRequest>(this, false);

            _dispatcher.Subscribe<ChangeScopes>(this, false);
            _dispatcher.Subscribe<CloseProject>(this, false);
            _dispatcher.Subscribe<RenameProject>(this, false);
            _dispatcher.Subscribe<ReopenProject>(this, false);
            _dispatcher.Subscribe<ResetSecret>(this, false);
        }

        // if we're creating a service command?
        public void Handle(NewProjectRequest message)
        {
            // if the recommendation is to factor over to this model, then it seems as-if
            // the aggregate command(s) and event(s) should be made *private*, rather than
            // accessed from "outside" the aggregate logic.
            var cmd = new StartProject { };
            var agg = new Aggregates.Project(cmd);
            agg.On(cmd);
            _repository.Save(agg);
        }

        public void Handle(ChangeScopes message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(CloseProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(RenameProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(ReopenProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(ResetSecret message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }
    }
}