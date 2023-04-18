using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Models
{
    public class Poll
    {
        public Poll()
        {
            Options = new HashSet<Option>();
			Voters = new HashSet<PollUser>();
		}

        [Key]
        public Int32 Id { get; set; }

        public string Question { get; set; } = null!;

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public virtual ICollection<Option> Options { get; set; }

        public virtual ICollection<PollUser> Voters { get; set; }

        public void AddVoter(ApplicationUser user)
        {
            PollUser puser = new PollUser(Id, user.Id);
            Voters.Add(puser);
            user.Polls.Add(puser);
        }

        public void Voted(ApplicationUser user)
        {
            Voters.Where(voter => voter.PollId == Id && voter.ApplicationUserId == user.Id).FirstOrDefault()!.Voted = true;
        }
	}
}
