using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore8.Model
{
    public class RefreshToken
    {
        [Key]
        public long id { get; set; }
        public required string Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }

    }
}
