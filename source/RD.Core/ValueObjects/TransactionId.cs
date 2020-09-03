using System;
using System.Security.Cryptography;
using System.Text;

namespace RD.Core.ValueObjects {
    public class TransactionId : IValueObject, IEquatable<TransactionId> {
        private readonly string _id;

        public TransactionId() => _id = Generate(false, 10);
        public TransactionId(string id) => _id = id;

        public static TransactionId Empty => new TransactionId(string.Empty);

        public bool Equals(TransactionId other) => other?._id == _id;

        public override bool Equals(object obj) => obj is TransactionId other && Equals(other);

        public override int GetHashCode() => _id.GetHashCode();

        public static bool operator ==(TransactionId left, TransactionId right) {
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

        public static bool operator !=(TransactionId left, TransactionId right) => !(left == right);

        public override string ToString() => _id;

        public object ToPrimitiveType() => _id;

        private static string Generate(bool isCaseSensitive, int length) {
            char[] chars;
            string a;

            if (isCaseSensitive)
                a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            else
                a = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            chars = a.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[length];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(length);
            foreach (var b in data) result.Append(chars[b % (chars.Length - 1)]);
            return result.ToString();
        }
    }
}