using Microsoft.EntityFrameworkCore;
using SilentAuction.Models;

namespace SilentAuction.Data
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }
        public DbSet<SilentAuction.Models.Item> Item { get; set; }

        // Tables in the database
        //public DbSet<Model> Model { get; set; }
    }
}
