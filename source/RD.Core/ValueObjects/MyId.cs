using System;

namespace RD.Core.ValueObjects {
    public class MyId : ValueObject<Guid>, IConvertible {
        public MyId() : this(Guid.NewGuid()) { }

        public MyId(Guid value) : base(value) { }

        public MyId(string value) : base(value) { }

        public static MyId Empty => new MyId(Guid.Empty);
    }
}