using Microsoft.EntityFrameworkCore;
using MVCapp.Models;

namespace MVCapp.Services
{
    public interface IPollService
    {
        List<Poll> GetPolls();
        List<Poll> GetPollsByUser(string userid);
		Poll GetPoll(Int32 id);
        bool CreatePoll(Poll poll);
        bool UpdatePoll(Poll poll);
        bool DeletePoll(Int32 id);
        void Vote(Poll poll, string userid, int optionid);
    }
}
