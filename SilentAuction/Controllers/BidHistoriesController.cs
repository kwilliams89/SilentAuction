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
    public class BidHistoriesController : Controller
    {
        private readonly AuctionContext _context;

        public BidHistoriesController(AuctionContext context)
        {
            _context = context;    
        }

        // GET: BidHistories
        public async Task<IActionResult> Index()
        {
            var auctionContext = _context.BidHistories.Include(b => b.Listing).Include(b => b.User);
            return View(await auctionContext.ToListAsync());
        }

        // GET: BidHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await _context.BidHistories
                .Include(b => b.Listing)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bidHistory == null)
            {
                return NotFound();
            }

            return View(bidHistory);
        }

        // GET: BidHistories/Create
        public IActionResult Create()
        {
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
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
                _context.Add(bidHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", bidHistory.UserId);
            return View(bidHistory);
        }

        // GET: BidHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await _context.BidHistories.SingleOrDefaultAsync(m => m.Id == id);
            if (bidHistory == null)
            {
                return NotFound();
            }
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", bidHistory.UserId);
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
                    _context.Update(bidHistory);
                    await _context.SaveChangesAsync();
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
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Id", bidHistory.ListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", bidHistory.UserId);
            return View(bidHistory);
        }

        // GET: BidHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await _context.BidHistories
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
            var bidHistory = await _context.BidHistories.SingleOrDefaultAsync(m => m.Id == id);
            _context.BidHistories.Remove(bidHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BidHistoryExists(int id)
        {
            return _context.BidHistories.Any(e => e.Id == id);
        }
    }
}
