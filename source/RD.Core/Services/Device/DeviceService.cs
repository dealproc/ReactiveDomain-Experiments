using System;

using RD.Core.Commands.Device;

using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.Services.Device {
    public class DeviceService : IHandle<Commands.Device.Activate>, IHandle<Commands.Device.Deactivate>, IHandle<Commands.Device.Provision> {
        private readonly IDispatcher _dispatcher;
        private readonly ICorrelatedRepository _repository;
        public DeviceService(IDispatcher dispatcher, ICorrelatedRepository repository) {
            _dispatcher = dispatcher;
            _repository = repository;

            _dispatcher.Subscribe<Activate>(this, false);
            _dispatcher.Subscribe<Deactivate>(this, false);
            _dispatcher.Subscribe<Provision>(this, false);
        }

        public void Handle(Activate message) {
            var agg = _repository.GetById<Aggregates.Device>((Guid) message.Id.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(Deactivate message) {
            var agg = _repository.GetById<Aggregates.Device>((Guid) message.Id.ToPrimitiveType(), message);
            agg.On(message);
            _repository.Save(agg);
        }

        public void Handle(Provision message) {
            var agg = new Aggregates.Device(message);
            agg.On(message);
            _repository.Save(agg);
        }
    }
}