using MyApiNetCore8.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyApiNetCore8.DTO.Request
{
    public class SignUpModel
    {
        [Required]
        public string userName { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required, MinLength(6)]
        public string password { get; set; }
        public string Avatar { get; set; }
    }
}
