using System;
using System.Collections.Generic;

using RD.Core.Commands.Project;

using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.Services.Project {
    public class ProjectService : IAutoActivate, IDisposable, IHandleCommand<NewProjectRequest>, IHandleCommand<Commands.Project.ChangeScopes>, IHandleCommand<Commands.Project.CloseProject>, IHandleCommand<Commands.Project.RenameProject>, IHandleCommand<Commands.Project.ReopenProject>, IHandleCommand<Commands.Project.ResetSecret> {
        private readonly IDispatcher _dispatcher;
        private readonly ICorrelatedRepository _repository;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private bool _hasBeenDisposed = false;

        public ProjectService(IDispatcher dispatcher, ICorrelatedRepository repository) {
            _dispatcher = dispatcher;
            _repository = repository;

            _disposables.Add(_dispatcher.Subscribe<NewProjectRequest>(this));

            _disposables.Add(_dispatcher.Subscribe<ChangeScopes>(this));
            _disposables.Add(_dispatcher.Subscribe<CloseProject>(this));
            _disposables.Add(_dispatcher.Subscribe<RenameProject>(this));
            _disposables.Add(_dispatcher.Subscribe<ReopenProject>(this));
            _disposables.Add(_dispatcher.Subscribe<ResetSecret>(this));
        }
        ~ProjectService() { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(_hasBeenDisposed) return;

            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
            _hasBeenDisposed = true;
        }

        // if we're creating a service command?
        public CommandResponse Handle(NewProjectRequest message)
        {
            // if the recommendation is to factor over to this model, then it seems as-if
            // the aggregate command(s) and event(s) should be made *private*, rather than
            // accessed from "outside" the aggregate logic.
            var cmd = new StartProject { };
            var agg = new Aggregates.Project(cmd);
            agg.On(cmd);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(ChangeScopes message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(CloseProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(RenameProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(ReopenProject message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(ResetSecret message) {
            var agg = _repository.GetById<Aggregates.Project>((Guid) message.ProjectId.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }
    }
}