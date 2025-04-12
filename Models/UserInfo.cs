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
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Role { get; set; }

        public UserInfo(string username, string password, string email, string role)
        {
            Username = username;
            Email = email;
            Role = role;
            Password = password;
        }

    }
}
