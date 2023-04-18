using System.ComponentModel.DataAnnotations;

namespace MVCapp.Models
{
    public class Option
    {
        [Key]
        public Int32 Id { get; set; }
        public string Ans { get; set; } = null!;
        public int Votes { get; set; } = 0;
    }
}
