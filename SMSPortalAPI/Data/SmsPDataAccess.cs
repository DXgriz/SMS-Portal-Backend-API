namespace SMSPortalAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using SMSPortalAPI.Models;

    public class SmsPDataAccess(DbContextOptions<SmsPDataAccess> opts) : DbContext(opts)
    {
        public DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public DbSet<SmsMessage> SmsMessages { get; set; } = null!;
    }
}
