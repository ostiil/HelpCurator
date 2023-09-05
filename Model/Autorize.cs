using System.ComponentModel.DataAnnotations;

namespace DP.Model
{
    public class Autorize
    {
        [Key]
        public int Id_record { get; set; }
        public string? Password { get; set; }
    }
}
