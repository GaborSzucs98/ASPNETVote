using System.ComponentModel.DataAnnotations;

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
        public int Id { get; set; }

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
            if (Voters.Contains(puser)) {
                return;
            }
            else
            {
                Voters.Add(puser);
                user.Polls.Add(puser);
            }
        }

        public Option GetOption(int id)
        {
            return Options.Single(op => op.Id == id);
        }

        public void Vote(string userid, int optionid)
        {
            GetOption(optionid).Votes++;
            Voters.Where(voter => voter.PollId == this.Id && voter.ApplicationUserId == userid).FirstOrDefault()!.Voted = true;
        }

        public int GetNumOfVoted() 
        {
            return Voters.Where(voter => voter.Voted == true).Count();
        }
	}
}
