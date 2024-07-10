using MyApiNetCore8.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyApiNetCore8.DTO.Request
{
    public class SignUpModel
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Column(TypeName = "ENUM('FEMALE', 'MALE', 'OTHER')")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender gender { get; set; }
        public DateTime Dob { get; set; } = DateTime.Now;
        [Required]
        public string userName { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required, MinLength(6)]
        public string password { get; set; }
    }
}
