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

        public DbSet<ItemMedia> ItemMedia { get; set; }

        public DbSet<Media> Media { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BidHistory> BidHistories { get; set; }

        public DbSet<Catagory> Catagories { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var itemMedia = modelBuilder.Entity<ItemMedia>();
            itemMedia.HasKey(itemMedia0 => new { itemMedia0.ItemId, itemMedia0.MediaId });

            itemMedia.HasOne(itemMedia0 => itemMedia0.Item)
                .WithMany(item => item.ItemMedia)
                .HasForeignKey(itemMedia0 => itemMedia0.ItemId);

            itemMedia.HasOne(itemMedia0 => itemMedia0.Media)
                .WithMany()
                .HasForeignKey(itemMedia0 => itemMedia0.MediaId);

            return;
        }
            // Tables in the database
            //public DbSet<Model> Model { get; set; }
        }
}
