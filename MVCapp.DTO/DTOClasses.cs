using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.DTOClasses
{
    public class UserDTO
    {
        public String Email { get; set; }

        public String Password { get; set; }
    }

    public class PollDTO
    {
		public int Id { get; set; }

		public string Question { get; set; }

		public DateTime Start { get; set; }

		public DateTime End { get; set; }

	}

    public class PollUserDTO
    {
		public int Id { get; set; }
		public int PollId { get; set; }
		public string ApplicationUserId { get; set; }
		public bool Voted { get; set; }
	}

    public class OptionDTO
    {
		public Int32 Id { get; set; }
		public string Ans { get; set; }
		public int Votes { get; set; }

		public int PollId { get; set; }
	}
}