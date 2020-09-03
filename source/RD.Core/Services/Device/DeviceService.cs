using System;
using System.Collections.Generic;

using RD.Core.Commands.Device;

using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.Services.Device {
    public class DeviceService : IAutoActivate, IDisposable, IHandleCommand<Commands.Device.Activate>, IHandleCommand<Commands.Device.Deactivate>, IHandleCommand<Commands.Device.Provision> {
        private readonly IDispatcher _dispatcher;
        private readonly ICorrelatedRepository _repository;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private bool _hasBeenDisposed = false;

        public DeviceService(IDispatcher dispatcher, ICorrelatedRepository repository) {
            _dispatcher = dispatcher;
            _repository = repository;

            _dispatcher.Subscribe<Activate>(this);
            _dispatcher.Subscribe<Deactivate>(this);
            _dispatcher.Subscribe<Provision>(this);
        }
        ~DeviceService() { Dispose(false); }

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

        public CommandResponse Handle(Activate message) {
            var agg = _repository.GetById<Aggregates.Device>((Guid) message.Id.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(Deactivate message) {
            var agg = _repository.GetById<Aggregates.Device>((Guid) message.Id.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }

        public CommandResponse Handle(Provision message) {
            var agg = new Aggregates.Device(message);
            agg.On(message);
            _repository.Save(agg);
            return message.Succeed();
        }
    }
}