using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bsnsapp.Models;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Bsnsapp.Models
{
    public class ChangePassword
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("User Id")]
        public string UserId { get; set; }
        [Required]
        [DisplayName("Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } 
        [Required]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DisplayName("Retype Password")]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }

        
      
    }
}