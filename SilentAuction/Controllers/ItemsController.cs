using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class ItemsController : Controller
    {
        private readonly AuctionContext _context;
        private readonly IHostingEnvironment _environment;

        public ItemsController(AuctionContext context,
            IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private static ItemViewModel ToViewModel(Item item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Sponsor = item.Sponsor.Name,
                Description = item.Description,
                Category = item.Category.Name,
                RetailPrice = item.RetailPrice.ToThaiCurrencyDisplayString()

            };
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var items = await _context.Items
                .AsNoTracking()
                .Include(item => item.Category)
                .Include(l => l.Sponsor)
                .ToListAsync();

            var itemViewModelQuery =
                from item in items
                select ToViewModel(item);

            var viewModels = itemViewModelQuery.ToList();
            return View(viewModels);
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
                .Include(i => i.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = ToViewModel(item);
            return View(viewModel);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SponsorId,CategoryId,Name,Description,Type,RetailPrice,MinimumBid")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
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

            var item = await _context.Items.Include(i => i.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
            ViewData["SponsorId"] = new SelectList(_context.Sponsors, "Id", "Name", item.SponsorId);
            return View(item);
        }

        public async Task<IActionResult> Upload(int itemId, ICollection<IFormFile> files)
        {
            foreach (var file in files)
            {
                var size = (int)file.Length;

                if (size <= 0)
                {
                    continue;
                }

                var content = ReadAllBytes(file);

                var media = new Media
                {
                    FileName = file.FileName,
                    Type = file.ContentType,
                    Content = content
                };

                _context.Media.Add(media);

                var itemMedia = new ItemMedia
                {
                    ItemId = itemId,
                    Media = media
                };

                _context.ItemMedia.Add(itemMedia);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Added {files.Count} images to the item.";

            return RedirectToAction(nameof(Edit), routeValues: new { id = itemId });
        }

        private static byte[] ReadAllBytes(IFormFile file)
        {
            var size = (int)file.Length;
            var buffer = new byte[size];

            using (var inputStream = file.OpenReadStream())
            {
                var bytesRemaining = size;
                var offset = 0;

                while (offset < size)
                {
                    var bytesRead = inputStream.Read(buffer, offset, bytesRemaining);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    offset += bytesRead;
                    bytesRemaining -= bytesRead;
                }
            }

            return buffer;
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SponsorId,CategoryId,Name,Description,Type,RetailPrice,MinimumBid")] Item item)
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
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
                .Include(i => i.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = ToViewModel(item);
            return View(viewModel);
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
