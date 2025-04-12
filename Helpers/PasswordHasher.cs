using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Serilog;

namespace LiveChartPlay.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return  BCrypt.Net.BCrypt.HashPassword(password);

        }

        public static bool VerifyPassword(string password , string hashedPassword)
        {
            Log.Information($"{HashPassword(password)}");
            Log.Information($"{hashedPassword}");

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
