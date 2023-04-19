using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Models
{
    public class VoteViewModel
    {
        public int pollid { get; set; }

        public Poll poll { get; set; } = null!;

		public int optionid { get; set; }

        public Option option { get; set; } = null!;
    }
}
