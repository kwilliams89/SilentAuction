using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using System;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class HomeController : Controller
    {
        private AuctionContext AuctionContext { get; }

        public HomeController(AuctionContext auctionContext)
        {
            AuctionContext = auctionContext ?? throw new ArgumentNullException(nameof(auctionContext));
        }

        public async Task<IActionResult> Index()
        {
            return View(await AuctionContext.Auctions.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Items()
        {
            return View();
        }
    }
}
