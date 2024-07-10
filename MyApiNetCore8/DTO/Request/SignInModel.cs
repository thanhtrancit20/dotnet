using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore8.DTO.Request
{
    //It look like User DTO class
    public class SignInModel
    {
        [Required]
        public string username { get; set; }

        [Required, MinLength(6)]
        public string password { get; set; }
    }
}
