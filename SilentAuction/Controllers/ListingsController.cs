using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class ListingsController : Controller
    {
        private AuctionContext AuctionContext { get; }

        public ListingsController(AuctionContext auctionContext)
        {
            AuctionContext = auctionContext ?? throw new ArgumentNullException(nameof(auctionContext));
        }

        private static ListingViewModel ToViewModel(Listing listing)
        {
            return new ListingViewModel
            {
                Id = listing.Id,
                Item = listing.Item.Name,
                Auction = listing.Auction.Name,
                Increment = listing.Increment.ToThaiCurrencyDisplayString(),
                MinimumBid = listing.MinimumBid.ToThaiCurrencyDisplayString()
            };
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            var listings = await AuctionContext.Listings
                .AsNoTracking()
                .Include(l => l.Auction)
                .Include(l => l.Item)
                .ToListAsync();

            var viewModelsQuery =
                from listing in listings
                select ToViewModel(listing);

            var viewModels = viewModelsQuery.ToList();
            return View(viewModels);
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await AuctionContext.Listings
                .Include(l => l.Auction)
                .Include(l => l.Item)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }
            var sponsor = AuctionContext.Sponsors.SingleOrDefaultAsync(sponsor0 => sponsor0.Id == listing.Item.SponsorId).Result;

            var myView = new BidHistoryViewModel {

                ListingId = listing.Id,
                MyListing = listing,
                CurrentBid = listing.MinimumBid,
                MinimumBid = listing.MinimumBid + listing.Increment,
                MySponsor = sponsor.Name
            };

            return View(myView);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            var viewModel = new ListingViewModel();
            viewModel.Auctions = new SelectList(AuctionContext.Auctions, "Id", "Name", viewModel.Auction);
            viewModel.Items = new SelectList(AuctionContext.Items, "Id", "Name", viewModel.Item);
            return View(viewModel);
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Auction))
            {
                ModelState.AddModelError("Auction", "The Auction field is empty.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(viewModel.Item))
                {
                    ModelState.AddModelError("Item", "The Item field is empty.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(viewModel.MinimumBid))
                    {
                        ModelState.AddModelError("MinimumBid", "The Starting bid is empty");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(viewModel.Increment))
                        {
                            ModelState.AddModelError("Increment", "The Increment field is empty.");
                        }
                        else
                        {
                            if (!int.TryParse(viewModel.Auction, out var auctionId))
                            {
                                ModelState.AddModelError("Auction", "Couldn't parse AuctionId");
                            }
                            else
                            {
                                if (!int.TryParse(viewModel.Item, out var itemId))
                                {
                                    ModelState.AddModelError("Item", "Couldn't parse ItemId");
                                }
                                else
                                {
                                    if (!decimal.TryParse(viewModel.MinimumBid, out var MinimumBid))
                                    {
                                        ModelState.AddModelError("MinimumBid", "Couldn't parse Starting Bid");
                                    }
                                    else
                                    {
                                        if (!decimal.TryParse(viewModel.Increment, out var increment))
                                        {
                                            ModelState.AddModelError("Increment", "Couldn't parse Increment");
                                        }
                                        else
                                        {
                                            var listing = new Listing
                                            {
                                                AuctionId = auctionId,
                                                ItemId = itemId,
                                                MinimumBid = MinimumBid,
                                                Increment = increment
                                            };

                                            AuctionContext.Add(listing);
                                            await AuctionContext.SaveChangesAsync();

                                            TempData["SuccessMessage"] = $"Successfully created listing #{listing.Id.ToString()}.";

                                            return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            viewModel.Auctions = new SelectList(AuctionContext.Auctions, "Id", "Name", viewModel.Auction);
            viewModel.Items = new SelectList(AuctionContext.Items, "Id", "Name", viewModel.Item);
            return View(viewModel);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await AuctionContext.Listings.SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            var itemName = AuctionContext.Items.SingleOrDefault(item => item.Id == listing.AuctionId).Name;
            var auctionName = AuctionContext.Auctions.SingleOrDefault(auction => auction.Id == listing.AuctionId).Name;
            var viewModel = new ListingViewModel
            {
                Id = listing.Id,
                Item = itemName,
                Auction = auctionName,
                Increment = listing.Increment.ToThaiCurrencyDisplayString(),
                MinimumBid = listing.MinimumBid.ToThaiCurrencyDisplayString()
            };

            viewModel.Auctions = new SelectList(AuctionContext.Auctions, "Id", "Name", viewModel.Auction);
            viewModel.Items = new SelectList(AuctionContext.Items, "Id", "Name", viewModel.Item);
            return View(viewModel);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ListingViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(viewModel.Auction))
            {
                ModelState.AddModelError("Auction", "The Auction field is empty.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(viewModel.Item))
                {
                    ModelState.AddModelError("Item", "The Item field is empty.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(viewModel.MinimumBid))
                    {
                        ModelState.AddModelError("MinimumBid", "The Starting bid is empty");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(viewModel.Increment))
                        {
                            ModelState.AddModelError("Increment", "The Increment field is empty.");
                        }
                        else
                        {
                            if (!int.TryParse(viewModel.Auction, out var auctionId))
                            {
                                ModelState.AddModelError("Auction", "Couldn't parse AuctionId");
                            }
                            else
                            {
                                if (!int.TryParse(viewModel.Item, out var itemId))
                                {
                                    ModelState.AddModelError("Item", "Couldn't parse ItemId");
                                }
                                else
                                {
                                    string MinimumBidInput = viewModel.MinimumBid;
                                    if (Regex.IsMatch(MinimumBidInput, @"^฿"))
                                    {
                                        MinimumBidInput = MinimumBidInput.Substring(1);
                                    }
                                    if (!decimal.TryParse(MinimumBidInput, out var MinimumBid))
                                    {
                                        ModelState.AddModelError("MinimumBid", "Couldn't parse Starting Bid");
                                    }
                                    else
                                    {
                                        string incrementInput = viewModel.Increment;
                                        if (Regex.IsMatch(incrementInput, @"^฿"))
                                        {
                                            incrementInput = incrementInput.Substring(1);
                                        }
                                        if (!decimal.TryParse(incrementInput, out var increment))
                                        {
                                            ModelState.AddModelError("Increment", "Couldn't parse Increment");
                                        }
                                        else
                                        {
                                            var listing = AuctionContext.Listings.SingleOrDefaultAsync(listing0 => listing0.Id == id).Result;

                                            listing.ItemId = itemId;
                                            listing.AuctionId = auctionId;
                                            listing.MinimumBid = MinimumBid;
                                            listing.Increment = increment;

                                            try
                                            {
                                                AuctionContext.Update(listing);
                                                await AuctionContext.SaveChangesAsync();
                                            }
                                            catch (DbUpdateConcurrencyException)
                                            {
                                                if (!ListingExists(listing.Id))
                                                {
                                                    return NotFound();
                                                }
                                                else
                                                {
                                                    throw;
                                                }
                                            }
                                            TempData["SuccessMessage"] = $"Successfully created listing #{listing.Id.ToString()}.";
                                            return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            viewModel.Auctions = new SelectList(AuctionContext.Auctions, "Id", "Name", viewModel.Auction);
            viewModel.Items = new SelectList(AuctionContext.Items, "Id", "Name", viewModel.Item);
            return View(viewModel);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await AuctionContext.Listings
                .Include(l => l.Auction)
                .Include(l => l.Item)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            var viewModel = ToViewModel(listing);
            return View(viewModel);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await AuctionContext.Listings.SingleOrDefaultAsync(m => m.Id == id);
            AuctionContext.Listings.Remove(listing);
            await AuctionContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ListingExists(int id)
        {
            return AuctionContext.Listings.Any(e => e.Id == id);
        }
    }
}
