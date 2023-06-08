using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVCapp.Persitence
{
    public class VotingDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Poll> Polls { get; set; } = null!;

        public VotingDbContext(DbContextOptions<VotingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Option> Option { get; set; } = null!;
	}
}
