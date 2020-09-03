using System;

using RD.Core.Commands.Project;
using RD.Core.Events.Project;
using RD.Core.ValueObjects;

using ReactiveDomain;
using ReactiveDomain.Messaging;

namespace RD.Core.Aggregates {
    public class Project : AggregateRoot {
        private ProjectId ProjectId { get; set; }
        private AccountId AccountId { get; set; }
        private string Name { get; set; }
        private string Secret { get; set; }
        private bool IsOpen { get; set; }

        public Project() : base()
        {
            RegisterRoutes();
        }

        public Project(ICorrelatedMessage source = null) : base(source)
        {
            RegisterRoutes();
        }

        public void RegisterRoutes() {
            Register<ProjectClosed>(Apply);
            Register<ProjectRenamed>(Apply);
            Register<ProjectReopened>(Apply);
            Register<ProjectStarted>(Apply);
            Register<ScopesChanged>(Apply);
            Register<SecretHasBeenReset>(Apply);
        }

        public void On(ChangeScopes command) {
            Raise(new ScopesChanged {
                ProjectId = ProjectId,
                AccountId = AccountId,
                Scopes = command.Scopes,
                Timestamp = command.Timestamp
            });
        }

        public void On(CloseProject command) {
            if (!IsOpen) { return; }

            Raise(new ProjectClosed {
                ProjectId = ProjectId,
                AccountId = AccountId,
                Timestamp = command.Timestamp
            });
        }

        public void On(RenameProject command) {
            if (Name.Equals(command.ProjectName)) return;

            Raise(new ProjectRenamed {
                ProjectId = ProjectId,
                AccountId = AccountId,
                OldProjectName = Name,
                NewProjectName = command.ProjectName,
                Timestamp = command.Timestamp
            });
        }

        public void On(ReopenProject command) {
            if (IsOpen) return;

            Raise(new ProjectReopened {
                ProjectId = ProjectId,
                AccountId = AccountId,
                Timestamp = command.Timestamp
            });
        }

        public void On(ResetSecret command) {
            Raise(new SecretHasBeenReset {
                ProjectId = command.ProjectId,
                AccountId = command.AccountId,
                OldSecret = Secret,
                NewSecret = Guid.NewGuid().ToString("N"),
                Timestamp = command.Timestamp
            });
        }

        public void On(StartProject command) {
            if (ProjectId == command.ProjectId) return;

            Raise(new ProjectStarted {
                ProjectId = command.ProjectId,
                AccountId = command.AccountId,
                Name = command.Name,
                ClientId = $"{Guid.NewGuid().ToString("N")}.apps.giftcardhosting.com",
                ClientSecret = Guid.NewGuid().ToString("N"),
                Timestamp = command.Timestamp
            });
        }

        private void Apply(ProjectClosed @event) {
            IsOpen = false;
        }

        private void Apply(ProjectRenamed @event) {
            Name = @event.NewProjectName;
        }

        private void Apply(ProjectReopened @event) {
            IsOpen = true;
        }

        private void Apply(ProjectStarted @event) {
            ProjectId = @event.ProjectId;
            AccountId = @event.AccountId;
            IsOpen = true;
            Name = @event.Name;
            Secret = @event.ClientSecret;
        }

        private void Apply(ScopesChanged e) { }

        private void Apply(SecretHasBeenReset @event) {
            Secret = @event.NewSecret;
        }
    }
}