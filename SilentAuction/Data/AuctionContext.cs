using Microsoft.EntityFrameworkCore;

namespace SilentAuction.Data
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        // Tables in the database
        //public DbSet<Model> Model { get; set; }
    }
}
