using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            return View(await AuctionContext.Auctions.ToListAsync());
        }


        public async Task<IActionResult> SilentAuction(int? id, string searchQuery, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = AuctionContext.Auctions.SingleOrDefaultAsync(auction0 => auction0.Id == id).Result;
            var endDate = auction.EndDate;
            var name = auction.Name;

            var listingsQuery =
                from listing in AuctionContext.Listings
                    .AsNoTracking()
                    .Include(listing0 => listing0.Item)
                        .ThenInclude(itemMedia0 => itemMedia0.ItemMedia)
                where listing.AuctionId == id
                select listing;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                listingsQuery = listingsQuery.Where(listing0 => listing0.Item.Name.Contains(searchQuery)
                    || listing0.Item.Description.Contains(searchQuery));
            }

            listingsQuery = listingsQuery.OrderBy(listing => listing.Item.Name);

            // List of items per page
            var pageSelectList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Selected = true, Text = "Five", Value = "5"},
                new SelectListItem {Selected = false, Text = "Ten" , Value = "10"}
            }
            );

            this.ViewBag.pageSelectList = new SelectList(pageSelectList, "Value", "Text");

            // items per page shown
            var pageSize = 5;

            // show items per page through PaginatedList
            var listings = await PaginatedList<Listing>.CreateAsync(listingsQuery, pageIndex ?? 1, pageSize);


            var viewModel = new AuctionViewModel
            {
                Id = id.Value,
                Listings = listings,
                SearchQuery = searchQuery,
                AuctionEndDate = endDate.ToString("D", new CultureInfo("en-EN")),
                AuctionName = name
            };

            return View(viewModel);
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await AuctionContext.Auctions
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
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                AuctionContext.Add(auction);
                await AuctionContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate")] Auction auction)
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
