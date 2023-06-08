using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MVCapp.Persitence.Services
{
    public class PollService : IPollService
    {
        private readonly VotingDbContext _context;
        public PollService(VotingDbContext context) { _context = context; }

        public List<Poll> GetPolls()
        {
            return _context.Polls.ToList();

        }

		public List<Poll> GetPollsByUser(string userid)
		{
			return _context.Polls.Where(poll => poll.Voters.Select(p => p.ApplicationUserId).Contains(userid)).ToList();
		}

		public Poll GetPoll(Int32 id)
        {
            var poll = _context.Polls.Single(v => v.Id == id);
            if(poll is null)
            {
                Trace.WriteLine("Nincs");
            }
            else
            {
				Trace.WriteLine(poll);
			}       
            return poll;
        }

        public Poll? CreatePoll(Poll poll)
        {
            try
            {
                _context.Add(poll);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return poll;
        }

        public bool UpdatePoll(Poll poll)
        {
            try
            {
                _context.Update(poll);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeletePoll(Int32 id)
        {
            var item = _context.Polls.Find(id);
            if (item == null)
            {
                return false;
            }

            try
            {
                _context.Remove(item);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public List<PollUser> GetPollUsers()
        {
            List<PollUser> pollUsers= new List<PollUser>();
            foreach (var item in _context.Polls)
            {
                pollUsers.AddRange(item.Voters);
            }
            return pollUsers;
        }

        public ApplicationUser GetApplicationUser(string userid)
        {
            return _context.Users.Single(u => u.Id == userid);
        }

        public bool AddVoter(int pollid, string userid)
        {
            try
            {
                GetPoll(pollid).AddVoter(GetApplicationUser(userid));
				_context.SaveChanges();
			}
            catch
            {
                return false;
            }
            return true;
        }

		public List<ApplicationUser> GetAllApplicationUser()
        {
            return _context.Users.ToList();
        }

		public void Vote(Poll poll, string userid, int optionid) {
            poll.Vote(userid, optionid);
            _context.SaveChanges();
        }

		public Option GetOption(int id)
		{
            return _context.Option.Single(o => o.Id == id);
		}

        public List<Option> GetAllOptions()
        {
            return _context.Option.ToList();
        }

		public Option? CreateOption(Option option)
		{
            try
            {
                _context.Add(option);
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return option;
		}
	}
}
