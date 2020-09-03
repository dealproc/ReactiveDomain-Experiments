using System;

namespace RD.Core.ValueObjects {
    public class DeviceId : ValueObject<Guid>, IConvertible {
        public DeviceId() : this(Guid.NewGuid()) { }

        public DeviceId(Guid value) : base(value) { }

        public DeviceId(string value) : base(value) { }

        public static DeviceId Empty => new DeviceId(Guid.Empty);
    }
}