using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bsnsapp.Models;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Bsnsapp.Models
{
    public class LoginPage
    {
        //public int Id { get; set; }
        [Required]
        [DisplayName("User Id")]
        public string UserID { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
    public class AdminLoginPage
    {
        //public int Id { get; set; }
        [Required]
        [DisplayName("User Id")]
        public string UserID { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}