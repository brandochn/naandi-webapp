using MySql.Data.MySqlClient;

namespace WebApi.ExtensionMethods
{
    public static class DataReaderExtensions
    {
        public static T GetValueOrNull<T>(this MySqlDataReader reader, int ordinal) where T : class
        {
            return !reader.IsDBNull(ordinal) ? reader.GetFieldValue<T>(ordinal) : null;
        }

        public static T? GetValueOrNullable<T>(this MySqlDataReader reader, int ordinal) where T : struct
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetFieldValue<T>(ordinal);
        }

        public static T GetValueOrDefault<T>(this MySqlDataReader reader, int ordinal) where T : struct
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetFieldValue<T>(ordinal);
        }
    }
}