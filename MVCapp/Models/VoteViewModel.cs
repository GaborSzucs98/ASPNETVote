using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Models
{
    public class VoteViewModel
    {
        [Required]
        public int pollid { get; set; } = -1;

        public Poll poll { get; set; } = null!;

        [Required]
		public int optionid { get; set; }

        public Option option { get; set; } = null!;
    }
}
