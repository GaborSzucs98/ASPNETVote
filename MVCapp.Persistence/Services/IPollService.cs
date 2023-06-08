namespace MVCapp.Persitence.Services
{
    public interface IPollService
    {
        List<Poll> GetPolls();
        List<Poll> GetPollsByUser(string userid);
		Poll GetPoll(Int32 id);
        Poll? CreatePoll(Poll poll);
        bool UpdatePoll(Poll poll);
        bool DeletePoll(Int32 id);
        Option GetOption(Int32 id);
        Option? CreateOption(Option option);
        List<Option> GetAllOptions();
		List<PollUser> GetPollUsers();
        ApplicationUser GetApplicationUser(string userid);
        List<ApplicationUser> GetAllApplicationUser();
		bool AddVoter(Int32 pollid, string userid);
        void Vote(Poll poll, string userid, int optionid);
    }
}
