using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVCapp.Models
{
    public class VotingDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Poll> Polls { get; set; } = null!;

        public VotingDbContext(DbContextOptions<VotingDbContext> options)
            : base(options)
        {
        }
    }
}
