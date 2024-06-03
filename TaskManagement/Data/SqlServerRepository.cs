using System.Data.SqlClient;

namespace TaskManagement.Data
{
    public class SqlServerRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly string _connectionString;

        public SqlServerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var items = new List<T>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = $"SELECT * FROM [{typeof(T).Name}]";

                using SqlCommand cmd = new(sql, conn);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new T();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var prop = item.GetType().GetProperty(reader.GetName(i));
                        if (prop != null && !Equals(reader.GetValue(i), DBNull.Value))
                        {
                            prop.SetValue(item, reader.GetValue(i), null);
                        }
                    }
                    items.Add(item);
                }
            }

            return items;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            T? item = null;

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                string sql = $"SELECT * FROM [{typeof(T).Name}] WHERE Id = @Id";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    item = new T();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var prop = item.GetType().GetProperty(reader.GetName(i));
                        if (prop != null && !Equals(reader.GetValue(i), DBNull.Value))
                        {
                            prop.SetValue(item, reader.GetValue(i), null);
                        }
                    }
                }
            }

            return item;
        }

        public async Task<int> AddAsync(T entity)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();
            var sql = SqlServerRepository<T>.GenerateInsertQuery(entity);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlServerRepository<T>.AddParameters(cmd, entity);
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var updateQuery = GenerateUpdateQuery();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(updateQuery, connection);
            foreach (var prop in typeof(T).GetProperties())
            {
                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity));
            }
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();
            string sql = $"DELETE FROM [{typeof(T).Name}] WHERE Id = @Id";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return await cmd.ExecuteNonQueryAsync();
        }

        private static string GenerateInsertQuery(T entity)
        {
            var properties = entity.GetType().GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => p.Name);
            var columnNames = string.Join(", ", properties);
            var parameterNames = string.Join(", ", properties.Select(p => "@" + p));
            return $"INSERT INTO [{typeof(T).Name}] ({columnNames}) VALUES ({parameterNames})";
        }

        private static string GenerateUpdateQuery()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => p.Name);

            var setClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));
            return $"UPDATE [{typeof(T).Name}] SET {setClause} WHERE Id = @Id";
        }

        private static void AddParameters(SqlCommand cmd, T entity)
        {
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (prop.Name != "Id")
                {
                    var value = prop.GetValue(entity) ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@" + prop.Name, value);
                }
            }
        }
    }

}
