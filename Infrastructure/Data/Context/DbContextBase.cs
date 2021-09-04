using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class DbContextBase : DbContext
    {
        public DbContextBase()
        {
        }

        public DbContextBase(DbContextOptions options)
            : base(options)
        {
        }
    }
}