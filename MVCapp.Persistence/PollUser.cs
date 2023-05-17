using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Persitence
{
	public class PollUser
	{
		public int Id { get; set; }

		[ForeignKey("Poll")]
		public int PollId { get; set; }

		[ForeignKey("ApplicationUser")]
		public string ApplicationUserId { get; set; } = null!;

		public bool Voted { get; set; }

		public PollUser() 
		{
			Voted = false;
		}

		public PollUser(int pollid, string userid)
		{ 
			PollId = pollid;
			ApplicationUserId = userid;
			Voted = false;
		}
	} 
}
