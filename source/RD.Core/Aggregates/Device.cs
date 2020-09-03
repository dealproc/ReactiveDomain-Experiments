using System;
using RD.Core.Commands.Device;
using RD.Core.Events.Device;
using RD.Core.ValueObjects;

using ReactiveDomain;
using ReactiveDomain.Messaging;

namespace RD.Core.Aggregates {
    public class Device : AggregateRoot {
        const int DEVICE_ID_OFFSET = 70000000;

        public Device() : base() 
        { 
            RegisterRoutes();
        }

        public Device(ICorrelatedMessage source = null) : base(source) 
        { 
            RegisterRoutes();
        }

        private void RegisterRoutes() {
            Register<Activated>(Apply);
            Register<Deactivated>(Apply);
            Register<Provisioned>(Apply);
        }


        private DeviceId DeviceId { get; set; }
        private AccountId AccountId { get; set; }
        private MyId MyId { get; set; }
        private string TID { get; set; }
        private bool Active { get; set; }

        public void On(Provision command) {
            if (DeviceId == command.Id) return;
            if (DeviceId != null) throw new Exception();

            Raise(new Provisioned {
                Id = command.Id,
                AccountId = command.AccountId,
                MyId = command.MyId,
                Description = command.Description,
                Timestamp = command.Timestamp,
                TID = (DEVICE_ID_OFFSET + command.DeviceIdOffset).ToString()
            });
        }

        public void On(Activate command) {
            if (DeviceId == null) throw new Exception();
            if (DeviceId != command.Id) throw new Exception();

            if (Active) return;

            Raise(new Activated {
                Id = command.Id,
                AccountId = command.AccountId,
                MyId = command.MyId,
                Timestamp = command.Timestamp
            });
        }

        public void On(Deactivate command) {
            if (DeviceId == null) throw new Exception();
            if (DeviceId != command.Id) throw new Exception();

            if (!Active) return;

            Raise(new Deactivated {
                Id = command.Id,
                AccountId = command.AccountId,
                MyId = command.MyId,
                Timestamp = command.Timestamp
            });
        }

        private void Apply(Provisioned evt) {
            DeviceId = evt.Id;
            AccountId = evt.AccountId;
            MyId = evt.MyId;
            TID = evt.TID;
            Active = true;
        }

        private void Apply(Activated evt) {
            Active = true;
        }

        private void Apply(Deactivated evt) {
            Active = false;
        }
    }
}