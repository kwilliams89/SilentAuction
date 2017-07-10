using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using System.Linq;
using System.Threading.Tasks;

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
            return View(await _context.BidHistories.ToListAsync());
        }

        // GET: BidHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidHistory = await _context.BidHistories
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
