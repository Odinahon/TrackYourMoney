using Microsoft.EntityFrameworkCore;
 
namespace BankAccounts.Models
{
    public class LoginContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LoginContext(DbContextOptions<LoginContext> options) : base(options) { }
        public DbSet<User> UserTable {get;set;}
        public DbSet<Transaction> Transaction{ get;set;}
    }
}
