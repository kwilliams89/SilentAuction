using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SilentAuction.Data;
using SilentAuction.Models;

namespace SilentAuction.Migrations
{
    [DbContext(typeof(AuctionContext))]
    partial class AuctionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SilentAuction.Models.Auction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("SilentAuction.Models.BidHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("ItemId");

                    b.Property<int>("ListingId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ListingId");

                    b.HasIndex("UserId");

                    b.ToTable("BidHistories");
                });

            modelBuilder.Entity("SilentAuction.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<decimal>("RetailPrice");

                    b.Property<int>("SponsorId");

                    b.Property<decimal>("StartingBid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.HasKey("Id");

                    b.HasIndex("SponsorId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SilentAuction.Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuctionId");

                    b.Property<decimal>("Increment");

                    b.Property<int>("ItemId");

                    b.Property<decimal>("StartingBid");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("ItemId");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("SilentAuction.Models.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content")
                        .IsRequired();

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int?>("ItemId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("SilentAuction.Models.Sponsor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("SilentAuction.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AutoBidAmt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<int>("Type");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SilentAuction.Models.BidHistory", b =>
                {
                    b.HasOne("SilentAuction.Models.Item")
                        .WithMany("BidHistories")
                        .HasForeignKey("ItemId");

                    b.HasOne("SilentAuction.Models.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SilentAuction.Models.User", "User")
                        .WithMany("BidHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SilentAuction.Models.Item", b =>
                {
                    b.HasOne("SilentAuction.Models.Sponsor", "Sponsor")
                        .WithMany("Items")
                        .HasForeignKey("SponsorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SilentAuction.Models.Listing", b =>
                {
                    b.HasOne("SilentAuction.Models.Auction", "Auction")
                        .WithMany("Listings")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SilentAuction.Models.Item", "Item")
                        .WithMany("Listings")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SilentAuction.Models.Media", b =>
                {
                    b.HasOne("SilentAuction.Models.Item")
                        .WithMany("Media")
                        .HasForeignKey("ItemId");
                });
        }
    }
}
