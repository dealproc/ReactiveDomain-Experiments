using System;

namespace RD.Core.ValueObjects {
    public class Scope : IValueObject, IEquatable<Scope> {
        private readonly string _id;

        public Scope() : this(string.Empty) { }
        public Scope(string id) => _id = id;

        public bool Equals(Scope other) => other._id == _id;

        public override bool Equals(object obj) => obj is Scope other && Equals(other);

        public override int GetHashCode() => _id.GetHashCode();

        public static bool operator ==(Scope left, Scope right) {
            // Check for null on left side.
            if (Object.ReferenceEquals(left, null)) {
                if (Object.ReferenceEquals(right, null)) {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return left.Equals(right);
        }

        public static bool operator !=(Scope left, Scope right) => !(left == right);

        public object ToPrimitiveType() => _id;

        public override string ToString() => _id;
    }
}