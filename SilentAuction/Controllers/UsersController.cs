using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilentAuction.Data;
using SilentAuction.Models;
using SilentAuction.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SilentAuction.Controllers
{
    public class UsersController : Controller
    {
        private readonly AuctionContext _context;

        public UsersController(AuctionContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(user => user.Role).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateOrEditUserViewModel
            {
                User = new User()
            };
            return await CreateOrEdit(viewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = viewModel.User;

                var dbUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    AutoBidAmt = user.AutoBidAmt
                };

                _context.Add(dbUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Successfully created user #{user.UserId.ToString()}.";

                return RedirectToAction("Index");
            }

            return await CreateOrEdit(viewModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new CreateOrEditUserViewModel
            {
                User = user
            };

            return await CreateOrEdit(viewModel);
        }

        private async Task<IActionResult> CreateOrEdit(CreateOrEditUserViewModel viewModel)
        {
            var roles = await _context.Roles.AsNoTracking().ToListAsync();
            var selectedRoleId = viewModel.User.RoleId;

            var roleViewModelsQuery =
                from role in roles
                select new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                    Selected = role.Id == selectedRoleId
                };

            viewModel.Roles = roleViewModelsQuery.ToList();

            return View(viewModel);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrEditUserViewModel viewModel)
        {
            var user = viewModel.User;

            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["SuccessMessage"] = $"Successfully edited user #{user.UserId.ToString()}.";

                return RedirectToAction("Index");
            }
            return await CreateOrEdit(viewModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
