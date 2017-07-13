using SilentAuction.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SilentAuction.Data
{
    public class TestSeedData
    {
        public static void Initialize(AuctionContext context)
        {
            context.Database.EnsureCreated();

            if (context.Auctions.Any())
            {
                return;
            }

            var auctions = new Auction[]
            {
                new Auction { StartDate = new DateTime(2017, 7, 10), EndDate = new DateTime(2017, 8, 15)}
            };
            foreach (Auction auction in auctions)
            {
                context.Auctions.Add(auction);
            }
            context.SaveChanges(); //Need to save everytime if using Ids

            var sponsors = new Sponsor[]
            {
                new Sponsor { Name = "WSU", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"}
            };
            foreach (Sponsor sponsor in sponsors)
            {
                context.Sponsors.Add(sponsor);
            }
            context.SaveChanges();

            var accommodationCatagory = new Catagory { Name = "Accommodation" };
            context.Catagories.Add(accommodationCatagory);

            var advertisingCatagory = new Catagory { Name = "Advertising" };
            context.Catagories.Add(advertisingCatagory);

            var diningCatagory = new Catagory { Name = "Dining" };
            context.Catagories.Add(diningCatagory);

            var entertainmentCatagory = new Catagory { Name = "Entertainment" };
            context.Catagories.Add(diningCatagory);

            var lifestyleCatagory = new Catagory { Name = "Lifestyle" };
            context.Catagories.Add(diningCatagory);
            context.SaveChanges();

            var items = new Item[]
            {
                new Item { Name = "Hotel Zero", Description = "Hotel Zero - A grand hotel", Catagory = accommodationCatagory, RetailPrice = 100, StartingBid = 20, SponsorId = sponsors[0].Id, Sponsor = sponsors[0]},
                new Item { Name = "Tour Zero", Description = "Tour Zero - A grand tour", Catagory = accommodationCatagory, RetailPrice = 60, StartingBid = 10, SponsorId = sponsors[0].Id, Sponsor = sponsors[0]}
            };
            foreach (Item item in items)
            {
                context.Items.Add(item);
            }
            context.SaveChanges();

            var listings = new Listing[]
            {
                new Listing { StartingBid = 20, Increment = 5, AuctionId = auctions[0].Id, Auction = auctions[0], ItemId = items[0].Id, Item = items[0]},
                new Listing { StartingBid = 10, Increment = 2, AuctionId = auctions[0].Id, Auction = auctions[0], ItemId = items[1].Id, Item = items[1]}
            };
            foreach (Listing listing in listings)
            {
                context.Listings.Add(listing);
            }

            var role = new Role { Id = RoleId.User, Name = "User" };
            context.Roles.Add(role);

            role = new Role { Id = RoleId.Administrator, Name = "Administrator" };
            context.Roles.Add(role);

            // Images
            // Found at: https://phab.phukethotelsassociation.com/silent-auction/
            var currentFolderPath = Assembly.GetEntryAssembly().Location;

            var poolVillaTisara1 = new Media
            {
                FileName = "PoolVillaTisara1.jpg",
                Type = "image/jpeg",
                Content = File.ReadAllBytes(Path.Combine(currentFolderPath, "../../../../SampleImages/Item1/2017_13_02_TRISARA__1090_FINAL-1900x1267.jpg"))
            };
            context.Media.Add(poolVillaTisara1);

            context.ItemMedia.Add(new ItemMedia { Item = items[0], Media = poolVillaTisara1 });

            var poolVillaTisara2 = new Media
            {
                FileName = "poolVillaTisara2.jpg",
                Type = "image/jpeg",
                Content = File.ReadAllBytes(Path.Combine(currentFolderPath, "../../../../SampleImages/Item1/H-OPV-10_Bathroom-1900x1501.jpg"))
            };
            context.Media.Add(poolVillaTisara2);

            context.ItemMedia.Add(new ItemMedia { Item = items[0], Media = poolVillaTisara2 });

            context.SaveChanges();
        }
    }
}
