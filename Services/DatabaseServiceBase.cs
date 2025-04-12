using CommunityToolkit.Mvvm.Messaging;
using LiveChartPlay.Services;
using Npgsql;
using Serilog;

public abstract class DatabaseServiceBase
{
    protected readonly string _connectionString;
    protected readonly IMessengerService _messenger;

    protected DatabaseServiceBase(string connectionString, IMessengerService messenger)
    {
        _connectionString = connectionString;
        _messenger = messenger;
    }


    protected async Task<NpgsqlConnection> CreateOpenConnectionAsync()
    {
        try
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }
        catch (Exception ex)
        {
            _messenger?.Publish("Failure DB connection {}");
            Log.Information(ex.ToString());
            throw; // Re-throw
        }
    }

    protected async Task<List<T>> ExecuteReaderAsync<T>(
    string sql,
    Func<NpgsqlDataReader, T> mapFunc,
    params NpgsqlParameter[] parameters)
    {
        try
        {
            using var conn = await CreateOpenConnectionAsync();
            using var cmd = new NpgsqlCommand(sql, conn);

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            using var reader = await cmd.ExecuteReaderAsync();

            var result = new List<T>();
            while (await reader.ReadAsync())
            {
                result.Add(mapFunc(reader));
            }
            return result;
        }
        catch (Exception ex)
        {
            _messenger?.Publish("DB Error");
            Log.Error(ex, $"Error executing SQL: {sql}");
            return new List<T>(); // or throw, based on your app policy
        }
    }

    protected async Task<int> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters)
    {
        try
        {
            using var conn = await CreateOpenConnectionAsync();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _messenger?.Publish("DB Error");
            Log.Error(ex, $"Error executing SQL: {sql}");
            return 0;
        }
    }



}
