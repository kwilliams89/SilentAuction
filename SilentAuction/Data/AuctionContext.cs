using Microsoft.EntityFrameworkCore;
using SilentAuction.Models;

namespace SilentAuction.Data
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) 
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Media> Media { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BidHistory> BidHistories { get; set; }

        public DbSet<Role> Roles { get; set; }
        // Tables in the database
        //public DbSet<Model> Model { get; set; }
    }
}
