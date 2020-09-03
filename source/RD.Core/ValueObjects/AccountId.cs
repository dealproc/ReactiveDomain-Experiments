using System;

namespace RD.Core.ValueObjects {
    public class AccountId : ValueObject<Guid>, IConvertible {
        public AccountId() : this(Guid.NewGuid()) { }

        public AccountId(Guid value) : base(value) { }

        public AccountId(string value) : base(value) { }

        public static AccountId Empty => new AccountId(Guid.Empty);
    }
}