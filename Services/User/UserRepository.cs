using LiveChartPlay.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Services
{
    
    public interface IUserRepository
    {
        Task<List<UserInfo>> GetAllUsersAsync();
        Task<UserInfo?> GetUserByUsernameAsync(string username);

        Task<bool> AnyUsersAsync();

        Task InsertUserAsync(UserInfo user);
    }
    public class UserRepository: DatabaseServiceBase, IUserRepository
    {
        private readonly IMessengerService _messenger;

        public UserRepository(IConfiguration configuration, IMessengerService messenger)
            : base(configuration.GetConnectionString("Default"), messenger)
        {
            _messenger = messenger;
        }

        public async Task<List<UserInfo>> GetAllUsersAsync()
        {
            return await ExecuteReaderAsync(
                "SELECT username, password, email, role FROM users",
                reader => new UserInfo(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    (UserRole)reader.GetInt32(3)
                )
            );
        }
        public async Task<UserInfo?> GetUserByUsernameAsync(string username)
        {
            var result = await ExecuteReaderAsync(
                "SELECT username, password, email, role FROM users WHERE username = @username",
                reader => new UserInfo(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    (UserRole)reader.GetInt32(3)
                ),
                new NpgsqlParameter("@username", username)
            );

            return result.FirstOrDefault();
        }

        public async Task<bool> AnyUsersAsync()
        {
            const string sql = "SELECT EXISTS (SELECT 1 FROM users)";
            var result = await ExecuteReaderAsync(sql, reader => reader.GetBoolean(0));
            return result.FirstOrDefault();
        }

        public async Task InsertUserAsync(UserInfo user)
        {
            const string sql = @"
                INSERT INTO users (username, password, email, role)
                VALUES (@username, @password, @email, @role)
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@username", user.Username),
                new NpgsqlParameter("@password", user.Password),
                new NpgsqlParameter("@email", user.Email),
                new NpgsqlParameter("@role", (int)user.Role)
            };

            await ExecuteNonQueryAsync(sql, parameters);
        }

    }
}
