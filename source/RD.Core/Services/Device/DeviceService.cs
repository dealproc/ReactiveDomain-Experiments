using System;
using System.Collections.Generic;
using RD.Core.Messages;
using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.Services.Device
{
    public class DeviceService :
        IHandleCommand<DeviceMsgs.Activate>,
        IHandleCommand<DeviceMsgs.Deactivate>,
        IHandleCommand<DeviceMsgs.Provision>,
        IDisposable
    {
        private readonly ICorrelatedRepository _repository;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();


        public DeviceService(IDispatcher bus, ICorrelatedRepository repository) {
            _repository = repository;
            _disposables.Add(bus.Subscribe<DeviceMsgs.Activate>(this));
            _disposables.Add(bus.Subscribe<DeviceMsgs.Provision>(this));
            _disposables.Add(bus.Subscribe<DeviceMsgs.Deactivate>(this));
        }

        public CommandResponse Handle(DeviceMsgs.Provision cmd) {
            var device = new Aggregates.Device(
                cmd.DeviceId,
                cmd.AccountId,
                cmd.MyId,
                cmd.TID,
                cmd.Description,
                cmd.Timestamp,
                cmd);
            _repository.Save(device);
            return cmd.Succeed();
        }
        public CommandResponse Handle(DeviceMsgs.Activate cmd) {
            var device = _repository.GetById<Aggregates.Device>(cmd.DeviceId,cmd);
            device.Activate(cmd.MyId, cmd.Timestamp); //n.b. is device is not found this will throw and the command system will catch it and convert it into a failed message
            _repository.Save(device);
            return cmd.Succeed();
        }

        public CommandResponse Handle(DeviceMsgs.Deactivate cmd) {
            var device = _repository.GetById<Aggregates.Device>(cmd.DeviceId,cmd);
            device.Deactivate(cmd.MyId, cmd.Timestamp);
            _repository.Save(device);
            return cmd.Succeed();
        }
        #region IDispose
        private bool _hasBeenDisposed = false;
        ~DeviceService() { Dispose(false); }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) {
            if (_hasBeenDisposed) return;

            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
            _hasBeenDisposed = true;
        }
        #endregion
    }
}