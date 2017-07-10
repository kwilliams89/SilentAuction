using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;

namespace SilentAuction.Controllers
{
    public class ListingsController : Controller
    {
        private readonly AuctionContext _context;

        public ListingsController(AuctionContext context)
        {
            _context = context;    
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            var auctionContext = _context.Listings.Include(l => l.Auction).Include(l => l.Item);
            return View(await auctionContext.ToListAsync());
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .Include(l => l.Auction)
                .Include(l => l.Item)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id");
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuctionId,ItemId,StartingBid,Increment")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", listing.AuctionId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", listing.ItemId);
            return View(listing);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", listing.AuctionId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", listing.ItemId);
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuctionId,ItemId,StartingBid,Increment")] Listing listing)
        {
            if (id != listing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction("Index");
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", listing.AuctionId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", listing.ItemId);
            return View(listing);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .Include(l => l.Auction)
                .Include(l => l.Item)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _context.Listings.SingleOrDefaultAsync(m => m.Id == id);
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ListingExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }
    }
}
