using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Krastev_It_API.Data
{
    public class KrastevItDbContext : IdentityDbContext<User>
    {
        public KrastevItDbContext(DbContextOptions<KrastevItDbContext> options)
            : base(options)
        {
        }
    }
}
