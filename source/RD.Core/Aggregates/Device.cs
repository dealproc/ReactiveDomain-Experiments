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
        
        public Device(DeviceId deviceId, AccountId accountId, MyId myId, string tid, string description, DateTime timestamp, ICorrelatedMessage source)
            : this() {
            Ensure.NotEmptyGuid((Guid)deviceId.ToPrimitiveType(), nameof(deviceId));
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
        private AccountId _accountId;
        private DeviceId _deviceId;
        /* unless they are used to validate a condition or set an event property there is no need for properties on an aggregate                
        private MyId MyId { get; set; }
        private string TID { get; set; }       
        */


        public void Activate(MyId myId, DateTime timeStamp) {
            if (_isActive) return;
            //todo validate myId and timestamp if required
            Raise(new DeviceMsgs.Activated(
               _deviceId,
                _accountId,
                myId,
                timeStamp));
        }

        public void Deactivate(MyId myId, DateTime timeStamp) {
            if (!_isActive) return;
            //todo validate myId and timestamp if required
            Raise(new DeviceMsgs.Deactivated(
                _deviceId,
                _accountId,
                myId,
                timeStamp));

        }

        private void Apply(DeviceMsgs.Provisioned evt) {
            
            Id = (Guid) evt.DeviceId.ToPrimitiveType(); // setting the Base Id for the aggregate instance is required by repository
            _deviceId = evt.DeviceId;
            _accountId = evt.AccountId; //I think this is right, or can the account change on this device during activations?
        }

        
    }
}