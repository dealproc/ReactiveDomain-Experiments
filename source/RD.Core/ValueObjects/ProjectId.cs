using System;

namespace RD.Core.ValueObjects {
    public class ProjectId : ValueObject<Guid>, IConvertible {
        public ProjectId() : this(Guid.NewGuid()) { }

        public ProjectId(Guid value) : base(value) { }

        public ProjectId(string value) : base(value) { }

        public static ProjectId Empty => new ProjectId(Guid.Empty);
    }
}