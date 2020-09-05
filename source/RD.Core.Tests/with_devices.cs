using System;
using System.Collections.Generic;
using System.Text;
using RD.Core.Aggregates;
using RD.Core.Messages;
using ReactiveDomain;
using ReactiveDomain.Messaging;
using Xunit;

namespace RD.Core.Tests {
    public class with_devices {

        const int DeviceIdOffset = 70000000;
        readonly Guid _deviceId = Guid.NewGuid();
        readonly Guid _accountId = Guid.NewGuid();
        readonly Guid _myId = Guid.NewGuid();
        readonly string _tid = "TestString";
        readonly string _description = "Description";
        readonly DateTime _time = DateTime.Now;
        readonly ICorrelatedMessage _source = new TestSource();

        [Fact]
        public void can_provision_a_device() {
            //given

            //when
            var device = GetDevice();

            //then
            var events = device.TakeEvents();
            Assert.Collection(events,
                evt => {
                    Assert.IsType<DeviceMsgs.Provisioned>(evt);
                    var provisioned = evt as DeviceMsgs.Provisioned;
                    Assert.Equal(_deviceId, provisioned.DeviceId);
                    Assert.Equal(_accountId, provisioned.AccountId);
                    Assert.Equal(_myId, provisioned.MyId);
                    Assert.Equal(DeviceIdOffset + _tid, provisioned.TID);
                    Assert.Equal(_description, provisioned.Description);
                    Assert.Equal(_time, provisioned.Timestamp);
                });

        }

        [Fact]
        public void can_activate_devices() {
            //given

            var device = GetDevice();
            // ReSharper disable once RedundantAssignment
            var events = device.TakeEvents();
            ((ICorrelatedEventSource)device).Source = new TestSource(); //taking events clears the source


            //when
            var myId = Guid.NewGuid();
            var activationTime = DateTime.Now;
            device.Activate(myId, activationTime);

            //then
            events = device.TakeEvents();
            Assert.Collection(events,
                evt => {
                    var @event = evt as DeviceMsgs.Activated;
                    Assert.NotNull(@event);
                    Assert.Equal(_deviceId, @event.DeviceId);
                    Assert.Equal(myId, @event.MyId);
                    Assert.Equal(activationTime, @event.Timestamp);
                });
        }
        [Fact]
        public void can_deactivate_devices() {
            //given
            var device = GetDevice();
            device.Activate(_myId, _time);
            // ReSharper disable once RedundantAssignment
            var events = device.TakeEvents();
            ((ICorrelatedEventSource)device).Source = new TestSource(); //taking events clears the source


            //when
            var myId = Guid.NewGuid();
            var activationTime = DateTime.Now;
            device.Deactivate(myId, activationTime);

            //then
            events = device.TakeEvents();
            Assert.Collection(events,
                evt => {
                    var @event = evt as DeviceMsgs.Deactivated;
                    Assert.NotNull(@event);
                    Assert.Equal(_deviceId, @event.DeviceId);
                    Assert.Equal(myId, @event.MyId);
                    Assert.Equal(activationTime, @event.Timestamp);
                });
        }
        [Fact]
        public void deactivation_is_idempotent() {
            //given
            var device = GetDevice();
            // ReSharper disable once RedundantAssignment
            var events = device.TakeEvents();
            ((ICorrelatedEventSource)device).Source = new TestSource(); //taking events clears the source


            //when
            var myId = Guid.NewGuid();
            var activationTime = DateTime.Now;
            device.Deactivate(myId, activationTime);

            //then
            events = device.TakeEvents();
            Assert.Empty(events);
            
        }
        [Fact]
        public void activation_is_idempotent() {
            //given
            var device = GetDevice();
            // ReSharper disable once RedundantAssignment
            device.Activate(_myId, _time);
            var events = device.TakeEvents();
            ((ICorrelatedEventSource)device).Source = new TestSource(); //taking events clears the source


            //when
            var myId = Guid.NewGuid();
            var activationTime = DateTime.Now;
            device.Activate(myId, activationTime);

            //then
            events = device.TakeEvents();
            Assert.Empty(events);
            
        }

        private Device GetDevice() {

            return new Device(_deviceId, _accountId, _myId, _tid, _description, _time, _source);

        }
    }
}
