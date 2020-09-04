using System;
using System.Collections.Generic;
using System.Linq;
using RD.Core.Aggregates;
using RD.Core.Messages;
using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging.Bus;

namespace RD.Core.ReadModels
{
    public class DevicesRm :
        ReadModelBase,
        IHandle<DeviceMsgs.Provisioned>,
        IHandle<DeviceMsgs.Activated>,
        IHandle<DeviceMsgs.Deactivated>
    {

        public DevicesRm(
            Func<string,IListener> getListener,
            Func<string,IStreamReader> getReader)
            : base(
                nameof(DevicesRm),
                 ()=> getListener(nameof(DevicesRm))) {
            long? checkpoint;
            using (var reader = getReader(nameof(DevicesRm))) {
                reader.EventStream.Subscribe<DeviceMsgs.Provisioned>(this);
                reader.EventStream.Subscribe<DeviceMsgs.Activated>(this);
                reader.EventStream.Subscribe<DeviceMsgs.Deactivated>(this);
                reader.Read<Device>();
                checkpoint = reader.Position;
            }

            EventStream.Subscribe<DeviceMsgs.Provisioned>(this);
            EventStream.Subscribe<DeviceMsgs.Activated>(this);
            EventStream.Subscribe<DeviceMsgs.Deactivated>(this);
            Start<Device>(checkpoint);
        }

        public List<DeviceModel> Devices = new List<DeviceModel>();

        public void Handle(DeviceMsgs.Provisioned evt) {
            Devices.Add(new DeviceModel{Id = evt.DeviceId,IsActive = false, Description = evt.Description});
        }

        public void Handle(DeviceMsgs.Activated evt) {
            var device = Devices.FirstOrDefault(d => d.Id ==  evt.DeviceId);
            if (device != null) {
                device.IsActive = true;
            }
        }

        public void Handle(DeviceMsgs.Deactivated evt) {
            var device = Devices.FirstOrDefault(d => d.Id ==  evt.DeviceId);
            if (device != null) {
                device.IsActive = false;
            }
        }

        public class DeviceModel {
            public Guid Id{ get; set; }
            public string Description{ get; set; }
            public bool IsActive { get; set; }
            
        }
    }
}
