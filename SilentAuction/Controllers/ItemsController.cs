using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        private static ItemViewModel ToViewModel(Item item, List<ItemMedia> itemMedia = null)
        {
            var mediaIds = new List<int>();

            if (itemMedia != null)
            {
                mediaIds = itemMedia.Select(itemMedia0 => itemMedia0.MediaId).ToList();
            }

            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Sponsor = item.Sponsor.Name,
                Description = item.Description,
                Category = item.Category.Name,
                RetailPrice = item.RetailPrice.ToThaiCurrencyDisplayString(),
                MediaIds = mediaIds,
                OfferExpires = item.OfferExpires,
                Terms = item.Terms
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
                .AsNoTracking()
                .Include(i => i.Sponsor)
                .Include(i => i.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var itemMedia = _context.ItemMedia
                .AsNoTracking()
                .Where(itemMedia0 => itemMedia0.ItemId == id)
                .ToList();

            var viewModel = ToViewModel(item, itemMedia);
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
        public async Task<IActionResult> Create([Bind("Id,SponsorId,CategoryId,Name,Description,Type,RetailPrice,MinimumBid,OfferExpires,Terms")] Item item)
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

            var item = await _context.Items
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(s => s.Sponsor)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            var itemMedia = _context.ItemMedia
               .AsNoTracking()
               .Where(itemMedia0 => itemMedia0.ItemId == id)
               .ToList();

            var viewModel = ToViewModel(item, itemMedia);
            viewModel.Sponsors = new SelectList(_context.Sponsors, "Id", "Name", viewModel.Sponsor);
            viewModel.Categories = new SelectList(_context.Categories, "Id", "Name", viewModel.Category);

            return View(viewModel);
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
        public async Task<IActionResult> Edit(int id, ItemViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!int.TryParse(viewModel.Sponsor, out var sponsorId))
            {
                ModelState.AddModelError("Sponsor", "Couldn't parse sponsor");
            }
            else if (!int.TryParse(viewModel.Category, out var categoryId))
            {
                ModelState.AddModelError("Category", "Couldn't parse category");
            }
            else if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                ModelState.AddModelError("Name", "The Name field is empty.");
            }
            else if (string.IsNullOrWhiteSpace(viewModel.Description))
            {
                ModelState.AddModelError("Description", "The Description field is empty.");
            }
            else if (string.IsNullOrWhiteSpace(viewModel.RetailPrice))
            {
                ModelState.AddModelError("RetailPrice", "The retail price field is empty.");
            }
            else if (string.IsNullOrWhiteSpace(viewModel.OfferExpires))
            {
                ModelState.AddModelError("OfferExpires", "The offer expires field is empty.");
            }
            else if (string.IsNullOrWhiteSpace(viewModel.Terms))
            {
                ModelState.AddModelError("Terms", "The terms and conditions field is empty.");
            }
            else
            {
                string retailPriceInput = viewModel.RetailPrice;

                if (Regex.IsMatch(retailPriceInput, @"^฿"))
                {
                    retailPriceInput = retailPriceInput.Substring(1);
                }

                if (!decimal.TryParse(retailPriceInput, out var retailPrice))
                {
                    ModelState.AddModelError("Retailprice", "Couldn't parse retail price");
                }
                else
                {
                    var item = _context.Items.SingleOrDefaultAsync(item0 => item0.Id == id).Result;

                    item.SponsorId = sponsorId;
                    item.Name = viewModel.Name;
                    item.Description = viewModel.Description;
                    item.CategoryId = categoryId;
                    item.RetailPrice = retailPrice;
                    item.OfferExpires = viewModel.OfferExpires;
                    item.Terms = viewModel.Terms;
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
                    TempData["SuccessMessage"] = $"Successfully changed listing #{item.Id.ToString()}.";
                    return RedirectToAction("Index");
                }
            }

            var itemMedia = _context.ItemMedia
               .AsNoTracking()
               .Where(itemMedia0 => itemMedia0.ItemId == id);

            viewModel.MediaIds = itemMedia.Select(itemMedia0 => itemMedia0.MediaId).ToList();
            viewModel.Sponsors = new SelectList(_context.Sponsors, "Id", "Name", viewModel.Sponsor);
            viewModel.Categories = new SelectList(_context.Categories, "Id", "Name", viewModel.Category);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int? mediaId, int? itemId)
        {
            if (mediaId == null || itemId == null)
            {
                return NotFound();
            }

            var itemMedia = await _context.ItemMedia
                .AsNoTracking()
                .SingleOrDefaultAsync(itemMedia0 => itemMedia0.MediaId == mediaId);

            var media = await _context.Media
                .AsNoTracking()
                .SingleOrDefaultAsync(media0 => media0.Id == mediaId);


            if (itemMedia == null || media == null)
            {
                return NotFound();
            }

            _context.ItemMedia.Remove(itemMedia);
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Deleted Media #{mediaId.ToString()}.";
            return RedirectToAction(nameof(Edit), new { id = itemId.Value });
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

            var itemMedia = _context.ItemMedia
               .AsNoTracking()
               .Where(itemMedia0 => itemMedia0.ItemId == id)
               .ToList();

            var viewModel = ToViewModel(item, itemMedia);
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
