using LiveChartPlay.Services;
using Npgsql;

public abstract class DatabaseServiceBase
{
    protected readonly string ConnectionString;
    protected readonly IMessengerService Messenger;

    protected DatabaseServiceBase(string connectionString, IMessengerService messenger)
    {
        ConnectionString = connectionString;
        Messenger = messenger;
    }


    protected async Task<NpgsqlConnection> CreateOpenConnectionAsync()
    {
        try
        {
            var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            return conn;
        }
        catch (Exception ex)
        {
            Messenger?.Publish("Failure DB connection");
            throw; // Re-throw
        }
    }
}
