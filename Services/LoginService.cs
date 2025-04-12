using LiveChartPlay.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Services
{
    public interface ILoginService
    {
        Task<UserInfo?> AuthenticateAsync(string username, string password);
    }
    public class LoginService : ILoginService
    {

        public Task<UserInfo?> AuthenticateAsync(string username, string password)
        {
            Log.Information($"{username} - {password}");
            if (!string.IsNullOrWhiteSpace(username) && password == "password")
            {
                return Task.FromResult<UserInfo?>(new UserInfo(username    , $"{username}@example.com", "User", password ));
            }

            return Task.FromResult<UserInfo?>(null);
        }
    }

}
