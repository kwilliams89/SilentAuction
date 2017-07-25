using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class ItemsController : Controller
    {
        private readonly AuctionContext _context;

        public ItemsController(AuctionContext context)
        {
            _context = context;    
        }

        private static ItemViewModel ToViewModel(Item item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Sponsor = item.Sponsor.Name,
                Description = item.Description,
                Catagory = item.Catagory.Name,
                RetailPrice = item.RetailPrice.ToString("C", new CultureInfo("th-TH"))

            };
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var auctionContext = _context.Items.Include(item => item.Catagory).Include(l => l.Sponsor);
            var itemsList = auctionContext.ToList();

            var viewModelsQuery =
                from items in itemsList
                select ToViewModel(items);

            var viewModels = viewModelsQuery.ToList();
            return View(viewModels);

           // var auctionContext = _context.Items.Include(item => item.Sponsor).Include(item => item.Catagory);
            //return View(await auctionContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Sponsor)
                .Include(i => i.Catagory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name");
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SponsorId,CatagoryId,Name,Description,Type,RetailPrice,StartingBid")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Name", item.CatagoryId);
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name", item.SponsorId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.Include(i => i.Catagory).SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Name", item.CatagoryId);
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name", item.SponsorId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SponsorId,CatagoryId,Name,Description,Type,RetailPrice,StartingBid")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name", item.SponsorId);
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Name", item.CatagoryId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Sponsor)
                .Include(i => i.Catagory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
