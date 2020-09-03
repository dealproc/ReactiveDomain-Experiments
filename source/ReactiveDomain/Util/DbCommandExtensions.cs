using System.Data.Common;

namespace ReactiveDomain.Util {
    public static class DbCommandExtensions {
        public static void AddWithValue(this DbCommand command, string parameterName, object value) {
            var param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            command.Parameters.Add(param);
        }
    }
}