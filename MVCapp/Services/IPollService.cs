using Microsoft.EntityFrameworkCore;
using MVCapp.Models;

namespace MVCapp.Services
{
    public interface IPollService
    {
        List<Poll> GetPolls();
        List<Poll> GetPollsByUser(ApplicationUser user);
		Poll GetPoll(Int32 id);
        bool CreatePoll(Poll voter);
        bool UpdatePoll(Poll voter);
        bool DeletePoll(Int32 id);
    }
}
