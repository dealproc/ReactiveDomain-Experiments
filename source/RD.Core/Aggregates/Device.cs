using System;
using RD.Core.Messages;
using RD.Core.ValueObjects;
using ReactiveDomain;
using ReactiveDomain.Messaging;
using ReactiveDomain.Util;

namespace RD.Core.Aggregates
{
    public class Device : AggregateRoot
    {
        const int DEVICE_ID_OFFSET = 70000000;

        public Device() {
            RegisterRoutes();
        }
        private void RegisterRoutes() {
            Register<DeviceMsgs.Activated>(_ => _isActive = true); //for small things these can be inline
            Register<DeviceMsgs.Deactivated>(_ => _isActive = false);
            Register<DeviceMsgs.Provisioned>(Apply); 
        }
        
        public Device(Guid deviceId, Guid accountId, Guid myId, string tid, string description, DateTime timestamp, ICorrelatedMessage source)
            : this() {
            Ensure.NotEmptyGuid(deviceId, nameof(deviceId));
            if (source.CausationId == Guid.Empty)
                Ensure.NotEmptyGuid(source.MsgId, nameof(source.MsgId));

            ((ICorrelatedEventSource)this).Source = source;
            //TODO: Validate all other fields before raising any event(s)
            Raise(
                new DeviceMsgs.Provisioned(
                             deviceId,
                             accountId,
                             myId,
                             DEVICE_ID_OFFSET + tid,
                             description,
                             timestamp));
        }


        private bool _isActive;
        private Guid _deviceId;
        /* unless they are used to validate a condition or set an event property there is no need for properties on an aggregate                
        private MyId MyId { get; set; }
        private string TID { get; set; }       
        */


        public void Activate(Guid myId, DateTime timeStamp) {
            if (_isActive) return;
            //todo validate myId and timestamp if required
            Raise(new DeviceMsgs.Activated(
               _deviceId,
                myId,
                timeStamp));
        }

        public void Deactivate(Guid myId, DateTime timeStamp) {
            if (!_isActive) return;
            //todo validate myId and timestamp if required
            Raise(new DeviceMsgs.Deactivated(
                _deviceId,
                myId,
                timeStamp));

        }

        private void Apply(DeviceMsgs.Provisioned evt) {
            
            Id =  evt.DeviceId; // setting the Base Id for the aggregate instance is required by repository
            _deviceId = evt.DeviceId;
        }

        
    }
}