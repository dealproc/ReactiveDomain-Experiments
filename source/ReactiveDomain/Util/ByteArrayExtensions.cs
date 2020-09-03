using System.Text;

namespace ReactiveDomain.Util
{
    public static class ByteArrayExtensions {
        public static string ToHex(this byte[] source, bool upperCase = false) {
            var result = new StringBuilder(source.Length * 2);

            for (int i = 0; i < source.Length; i++)
                result.Append(source[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}