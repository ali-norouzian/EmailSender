using EmailSender.Data.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        public DbSet<EmailSendingStatus> EmailSendingStatus { get; set; }
        public DbSet<EmailSendingGroup> EmailSendingGroup { get; set; }

    }
}
