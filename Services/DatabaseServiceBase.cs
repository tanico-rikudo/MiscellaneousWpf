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
}
