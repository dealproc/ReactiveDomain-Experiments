using System;
using System.Reflection;

namespace RD.Core.ValueObjects {
    public abstract class ValueObject<T> : IValueObject, IEquatable<ValueObject<T>>, IConvertible {
        protected readonly T _value;

        public ValueObject(T value) => _value = value;
        public ValueObject(string value) {
            if (typeof(T) == typeof(Guid)) {
                _value = (T) Activator.CreateInstance(typeof(Guid), new [] { value });
                return;
            }

            _value = (T) Convert.ChangeType(value, typeof(T));
        }

        public bool Equals(ValueObject<T> other) {
            if (other == null) { return false; }
            if (other._value == null && _value == null) { return true; }

            return other._value.Equals(_value);
        }

        public override bool Equals(object obj) => obj is ValueObject<T> other && Equals(other);

        public object ToPrimitiveType() => _value;

        public override string ToString() {
            if (typeof(T) == typeof(Guid)) {
                var info = _value.GetType().GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, new [] { typeof(string) }, null);
                return (string) info.Invoke(_value, new [] { "N" });
            }
            return _value.ToString();
        }

        public override int GetHashCode() => _value.GetHashCode();

        public TypeCode GetTypeCode() => Type.GetTypeCode(typeof(T));

        public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(_value, provider);

        public byte ToByte(IFormatProvider provider) => Convert.ToByte(_value, provider);

        public char ToChar(IFormatProvider provider) => Convert.ToChar(_value, provider);

        public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(_value, provider);

        public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(_value, provider);

        public double ToDouble(IFormatProvider provider) => Convert.ToDouble(_value, provider);

        public short ToInt16(IFormatProvider provider) => Convert.ToInt16(_value, provider);

        public int ToInt32(IFormatProvider provider) => Convert.ToInt32(_value, provider);

        public long ToInt64(IFormatProvider provider) => Convert.ToInt64(_value, provider);

        public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(_value, provider);

        public float ToSingle(IFormatProvider provider) => Convert.ToSingle(_value, provider);

        public string ToString(IFormatProvider provider) => Convert.ToString(_value, provider);

        public object ToType(Type conversionType, IFormatProvider provider) {
            if (conversionType == typeof(bool)) {
                return this.ToBoolean(provider);
            } else if (conversionType == typeof(byte)) {
                return this.ToBoolean(provider);
            } else if (conversionType == typeof(char)) {
                return this.ToChar(provider);
            } else if (conversionType == typeof(DateTime)) {
                return this.ToDateTime(provider);
            } else if (conversionType == typeof(decimal)) {
                return this.ToDecimal(provider);
            } else if (conversionType == typeof(double)) {
                return this.ToDouble(provider);
            } else if (conversionType == typeof(Int16)) {
                return this.ToInt16(provider);
            } else if (conversionType == typeof(Int32)) {
                return this.ToInt32(provider);
            } else if (conversionType == typeof(Int64)) {
                return this.ToInt64(provider);
            } else if (conversionType == typeof(SByte)) {
                return this.ToSByte(provider);
            } else if (conversionType == typeof(float)) {
                return this.ToSingle(provider);
            } else if (conversionType == typeof(string)) {
                return this.ToString(provider);
            }

            throw new InvalidOperationException($"Cannot handle conversion to type {conversionType.Name}.");
        }

        public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(_value, provider);

        public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(_value, provider);
        public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(_value, provider);

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right) {
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

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right) => !(left == right);
    }
}