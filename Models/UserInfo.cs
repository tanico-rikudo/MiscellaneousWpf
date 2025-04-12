using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Models
{
    public class UserInfo
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.None;

        public UserInfo(string username, string password, string email, UserRole role)
        {
            Username = username;
            Email = email;
            Role = role;
            Password = password;
        }

    }
}
