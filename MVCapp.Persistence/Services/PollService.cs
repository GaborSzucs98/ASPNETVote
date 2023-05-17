using Microsoft.EntityFrameworkCore;

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
            return _context.Polls.Single(v => v.Id == id);
        }

        public bool CreatePoll(Poll poll)
        {
            try
            {
                _context.Add(poll);
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

        public void Vote(Poll poll, string userid, int optionid) {
            poll.Vote(userid, optionid);
            _context.SaveChanges();
        }
    }
}
