using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Headspring {
    [Serializable, DebuggerDisplay("{DisplayName} - {Value}")]
    public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
        where TEnumeration : Enumeration<TEnumeration> {
        protected Enumeration(int value, string displayName)
            : base(value, displayName) {
        }

        public static TEnumeration FromInt32(int value) => FromValue(value);

        public static bool TryFromInt32(int listItemValue, out TEnumeration result) =>
            TryParse(listItemValue, out result);
    }

    [Serializable, DebuggerDisplay("{DisplayName} - {Value}"),
     DataContract(Namespace = "http://github.com/HeadspringLabs/Enumeration/5/13")]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        protected Enumeration(TValue value, string displayName) {
            if (value == null) throw new ArgumentNullException();

            Value = value;
            DisplayName = displayName;
        }

        [field: DataMember(Order = 0)] public TValue Value { get; }

        [field: DataMember(Order = 1)] public string DisplayName { get; }

        public int CompareTo(TEnumeration other) =>
            Value.CompareTo(other == default(TEnumeration) ? default : other.Value);

        public bool Equals(TEnumeration other) => other != null && ValueEquals(other.Value);

        public sealed override string ToString() => DisplayName;

        public static TEnumeration[] GetAll() => Enumerations.Value;

        private static TEnumeration[] GetEnumerations() {
            var enumerationType = typeof(TEnumeration);
            return enumerationType
                   .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                   .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                   .Select(info => info.GetValue(null))
                   .Cast<TEnumeration>()
                   .ToArray();
        }

        public override bool Equals(object obj) => Equals(obj as TEnumeration);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator
            ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) => Equals(left, right);

        public static bool operator
            !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) => !Equals(left, right);

        public static TEnumeration FromValue(TValue value) => Parse(value, "value", item => item.Value.Equals(value));

        public static TEnumeration Parse(string displayName) =>
            Parse(displayName, "display name", item => item.DisplayName == displayName);

        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result) {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate) {
            TEnumeration result;

            if (!TryParse(predicate, out result)) {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description,
                    typeof(TEnumeration));
                throw new ArgumentException(message, "value");
            }

            return result;
        }

        public static bool TryParse(TValue value, out TEnumeration result) =>
            TryParse(e => e.ValueEquals(value), out result);

        public static bool TryParse(string displayName, out TEnumeration result) =>
            TryParse(e => e.DisplayName == displayName, out result);

        protected virtual bool ValueEquals(TValue value) => Value.Equals(value);
    }
}