using Microsoft.EntityFrameworkCore;

namespace webApiWIthJWT.Data
{
    public class databaseContext : DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options): base (options)
        {

        }
        public DbSet<UserModel> user { get; set; }
    }
}
