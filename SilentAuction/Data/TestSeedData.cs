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

            // Auctions
            var auctions = new Auction[]
            {
                new Auction { Name = "Auction of August 2017", StartDate = new DateTime(2017, 7, 10), EndDate = new DateTime(2017, 8, 15)}
            };
            foreach (Auction auction in auctions)
            {
                context.Auctions.Add(auction);
            }
            context.SaveChanges(); //Need to save everytime if using Ids

            // Sponsors
            var sponsors = new Sponsor[]
            {
                // Accommodation
                new Sponsor { Name = "Trisara", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "Le Meridien Phuket Beach Resort", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "Andara Resort & Villas", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "The Surin Phuket", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "Mövenpick Resort, Bangtao Beach Phuket", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},

                // Advertising
                new Sponsor { Name = "Khao Phuket", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "Novosti Phuket", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "The Phuket News TV", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},

                // Dining (+Accomodation +Lifestyle)
                new Sponsor { Name = "Burasari Resort", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
                new Sponsor { Name = "The Siam Supper Club", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},

                // Entertainment
                new Sponsor { Name = "H3 Digital Limited", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},

                // Lifestyle (+Dinning, + Accomodation)
                new Sponsor { Name = "Novotel Phuket Kamala Beach", StartDate = new DateTime(2017, 7, 10), Phone = "5555555555"},
            };
            foreach (Sponsor sponsor in sponsors)
            {
                context.Sponsors.Add(sponsor);
            }
            context.SaveChanges();

            // Catagories
            var accommodationCatagory = new Category { Name = "Accommodation" };
            context.Categories.Add(accommodationCatagory);

            var advertisingCatagory = new Category { Name = "Advertising" };
            context.Categories.Add(advertisingCatagory);

            var diningCatagory = new Category { Name = "Dining" };
            context.Categories.Add(diningCatagory);

            var entertainmentCatagory = new Category { Name = "Entertainment" };
            context.Categories.Add(entertainmentCatagory);

            var lifestyleCatagory = new Category { Name = "Lifestyle" };
            context.Categories.Add(lifestyleCatagory);
            context.SaveChanges();

            // Items
            var items = new Item[]
            {
                // Accommodation
                new Item { Name = "2 Nights Accommodation in an Ocean View Pool Villa at Trisara", Description = "Enjoy a two night stay in a spectacular Ocean View Pool Villa at Trisara, inclusive of 2x Trisara spa treatments, daily breakfast and the Trisara Sunday Jazz Brunch for 2.", Category = accommodationCatagory, RetailPrice = 100, SponsorId = sponsors[0].Id, Sponsor = sponsors[0]},
                new Item { Name = "2 Nights Accommodation at Le Meridien Phuket Beach Resort + 2 Dinners", Description = "Enjoy a 3 day 2 night stay for two persons at the Le Meridien Phuket Beach Resort in an Ocean Front Deluxe Suite including daily buffet breakfast and one dinner at Portofino including a bottle of house wine and one dinner at Ariake including a carafe of Sake.", Category = accommodationCatagory, RetailPrice = 60, SponsorId = sponsors[1].Id, Sponsor = sponsors[1]},
                new Item { Name = "1 Night Accommodation at Andara Resort + In-Suite BBQ w/ Champagne", Description = "Enjoy a 1-night stay in a 4-bedroom Pool Suite at Andara Resort & Villas with daily breakfast at SILK Restaurant and a BBQ for 8 persons in your Suite Room with your own private chef accompanied by 2 bottles of Louis Roederer Champagne.", Category = accommodationCatagory, RetailPrice = 60, SponsorId = sponsors[2].Id, Sponsor = sponsors[2]},
                new Item { Name = "3 Nights Accommodation at The Surin Phuket", Description = "Enjoy a 3 night stay for two persons at The Surin Phuket in a Beach Deluxe Suite including daily buffet breakfast with signature dinner & spa treatment.", Category = accommodationCatagory, RetailPrice = 60, SponsorId = sponsors[3].Id, Sponsor = sponsors[3]},
                new Item { Name = "2 Nights Accommodation at Mövenpick Resort, Bangtao Beach Phuket", Description = "Enjoy a 2-night stay in a Seaview Pool Suite 1 Bedroom inclusive of breakfast for two at The Palm Cuisine at the Mövenpick Resort, Bangtao Beach Phuket.", Category = accommodationCatagory, RetailPrice = 60, SponsorId = sponsors[4].Id, Sponsor = sponsors[4]},

                // Advertising
                new Item { Name = "Half Page Advert in Khao Phuket (The Phuket News in Thai)", Description = "Receive one 1/2 page advertisement — 274mm (w) x 189mm (h) – in Khao Phuket (The Phuket News in Thai), including artwork and translation.", Category = advertisingCatagory, RetailPrice = 60, SponsorId = sponsors[5].Id, Sponsor = sponsors[5]},
                new Item { Name = "Half Page Advert in Novosti Phuket (The Phuket News in Russian)", Description = "Receive one 1/2 page advertisement — 274mm (w) x 189mm (h) – in Novosti Phuket (The Phuket News in Russian), including artwork and translation.", Category = advertisingCatagory, RetailPrice = 60, SponsorId = sponsors[6].Id, Sponsor = sponsors[6]},
                new Item { Name = "TV Advertisement in The Phuket News TV", Description = "Receive a 1 month x 10 second fixed graphic advertisement on The Phuket News TV.", Category = advertisingCatagory, RetailPrice = 60, SponsorId = sponsors[7].Id, Sponsor = sponsors[7]},

                // Dining (+Accomodation +Lifestyle)
                new Item { Name = "Eternity Spa Package (3.5 hrs) and Set Dinner for 2 at Burasari Resort, Phuket", Description = "Enjoy an Eternity Spa Package (lasting three and a half hours) as well as a set dinner for 2 persons at the Burasari Resort in Patong, Phuket.", Category = diningCatagory, RetailPrice = 60, SponsorId = sponsors[8].Id, Sponsor = sponsors[8]},
                new Item { Name = "THB 5,000 Voucher for use The Siam Supper Club", Description = "The winning bidder will get to enjoy THB 5,000 during a dining and/or drinking experience at The Siam Supper Club.", Category = diningCatagory, RetailPrice = 60, SponsorId = sponsors[9].Id, Sponsor = sponsors[9]},

                // Entertainment
                new Item { Name = "Nuvo AccentPLUS1 6.5″ In-Ceiling Speakers", Description = "Fill the room with impeccably clean, full sound with this set of Nuvo AccentPLUS1 6.5″ In-Ceiling Speakers.", Category = entertainmentCatagory, RetailPrice = 60, SponsorId = sponsors[10].Id, Sponsor = sponsors[10]},

                // Lifestyle (+Dinning, + Accomodation)
                new Item { Name = "2 Nights Accommodation at Novotel Phuket Kamala Beach + Dinner + Spa", Description = "Enjoy a 3 day 2 night stay for two persons at the Novotel Phuket Kamala Beach in a One Bedroom Pool Villa including daily breakfast, a romantic dinner for 2 at On The Roof and a one hour spa treatment for 2.", Category = lifestyleCatagory, RetailPrice = 60, SponsorId = sponsors[11].Id, Sponsor = sponsors[11]},
            };
            foreach (Item item in items)
            {
                context.Items.Add(item);
            }
            context.SaveChanges();

            // Listings
            var listings = new Listing[]
            {
                new Listing { MinimumBid = 20, Increment = 5, AuctionId = auctions[0].Id, ItemId = items[0].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[1].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[2].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[3].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[4].Id},

                new Listing { MinimumBid = 20, Increment = 5, AuctionId = auctions[0].Id, ItemId = items[5].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[6].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[7].Id},

                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[8].Id},
                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[9].Id},

                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[10].Id},

                new Listing { MinimumBid = 10, Increment = 2, AuctionId = auctions[0].Id, ItemId = items[11].Id}
            };
            foreach (Listing listing in listings)
            {
                context.Listings.Add(listing);
            }

            // User Roles
            var role = new Role { Id = RoleId.User, Name = "User" };
            context.Roles.Add(role);

            role = new Role { Id = RoleId.Administrator, Name = "Administrator" };
            context.Roles.Add(role);

            // Images
            // Found at: https://phab.phukethotelsassociation.com/silent-auction/
            // Media types: https://en.wikipedia.org/wiki/Media_type
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
