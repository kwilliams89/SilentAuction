using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class AuctionsController : Controller
    {
        private AuctionContext AuctionContext { get; }

        public AuctionsController(AuctionContext auctionContext)
        {
            AuctionContext = auctionContext ?? throw new ArgumentNullException(nameof(auctionContext));
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
            var auctionContext = AuctionContext.Auctions.Include(a => a.Listing);
            return View(await auctionContext.ToListAsync());
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await AuctionContext.Auctions
                .Include(a => a.Listing)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        public IActionResult Create()
        {
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id");
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListingId,StartDate,EndDate")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                AuctionContext.Add(auction);
                await AuctionContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", auction.ListingId);
            return View(auction);
        }

        // GET: Auctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await AuctionContext.Auctions.SingleOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", auction.ListingId);
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListingId,StartDate,EndDate")] Auction auction)
        {
            if (id != auction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AuctionContext.Update(auction);
                    await AuctionContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.Id))
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
            ViewData["ListingId"] = new SelectList(AuctionContext.Listings, "Id", "Id", auction.ListingId);
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await AuctionContext.Auctions
                .Include(a => a.Listing)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await AuctionContext.Auctions.SingleOrDefaultAsync(m => m.Id == id);
            AuctionContext.Auctions.Remove(auction);
            await AuctionContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AuctionExists(int id)
        {
            return AuctionContext.Auctions.Any(e => e.Id == id);
        }
    }
}
