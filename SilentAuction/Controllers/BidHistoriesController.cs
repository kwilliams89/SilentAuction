using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System.Text.RegularExpressions;

namespace SilentAuction.Controllers
{
    public class BidHistoriesController : Controller
    {
        private AuctionContext AuctionContext { get; }

        public BidHistoriesController(AuctionContext auctionContext)
        {
            AuctionContext = auctionContext ?? throw new ArgumentNullException(nameof(auctionContext));
        }

        // GET: BidHistories
        public async Task<IActionResult> Index()
        {
            var bidhistories = await AuctionContext.BidHistories.AsNoTracking()
                .Include(l => l.Listing)
                .Include(l => l.User).ToListAsync();
            return View(bidhistories);
        }

        // GET: BidHistories/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var bidHistory = await AuctionContext.BidHistories
                .Include(b => b.Listing)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.Id == id);

            var bidListing = await AuctionContext.Listings.Include(b => b.Item).SingleOrDefaultAsync(m => m.Id == bidHistory.ListingId);

            bidHistory.Listing = bidListing;
            bidHistory.User.Phone = Regex.Replace(bidHistory.User.Phone.ToString(), @"^(...)(...)(....)$", "$1-$2-$3");

            if (bidHistory == null)
            {
                return NotFound();
            }

            return View(bidHistory);
        }

        // GET: BidHistories/Create
        public IActionResult Create()
        {
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id");
            ViewData["UserId"] = new SelectList(AuctionContext.Users, "UserId", "Email");
            return View();
        }

        // POST: BidHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ListingId,Date,Amount")] BidHistory bidHistory)
        {
            if (ModelState.IsValid)
            {
                AuctionContext.Add(bidHistory);
                await AuctionContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(AuctionContext.Users, "UserId", "Email", bidHistory.UserId);
            return View(bidHistory);
        }

        // GET: BidHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await AuctionContext.BidHistories.SingleOrDefaultAsync(m => m.Id == id);
            if (bidHistory == null)
            {
                return NotFound();
            }
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(AuctionContext.Users, "UserId", "Email", bidHistory.UserId);
            return View(bidHistory);
        }

        // POST: BidHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ListingId,Date,Amount")] BidHistory bidHistory)
        {
            if (id != bidHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AuctionContext.Update(bidHistory);
                    await AuctionContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidHistoryExists(bidHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(AuctionContext.Users, "UserId", "Email", bidHistory.UserId);
            return View(bidHistory);
        }

        // GET: BidHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await AuctionContext.BidHistories
                .Include(b => b.Listing)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bidHistory == null)
            {
                return NotFound();
            }

            return View(bidHistory);
        }

        // POST: BidHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bidHistory = await AuctionContext.BidHistories.SingleOrDefaultAsync(m => m.Id == id);
            AuctionContext.BidHistories.Remove(bidHistory);
            await AuctionContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BidHistoryExists(int id)
        {
            return AuctionContext.BidHistories.Any(e => e.Id == id);
        }
    }
}
