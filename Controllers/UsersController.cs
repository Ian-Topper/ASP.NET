using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DroneWorks.Models;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace DroneWorks.Controllers
{

    public class UsersController : Controller
    {
        private readonly Team116DBContext _context;

        public UsersController(Team116DBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserOrderDetail()
        {
            int userPK = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var itemitem = _context.WorksUser
                .Where(c => c.WorksOrder.Count > 0)
                .Where(u => u.UserPk == userPK)
                .Include(cust => cust.WorksOrder)
                .ThenInclude(order => order.OrderItem)
                .ThenInclude(detail => detail.ProdFkNavigation);
            return View(await itemitem.ToListAsync());
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var team116DBContext = _context.WorksUser.Include(w => w.RoleFkNavigation);
            return View(await team116DBContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksUser = await _context.WorksUser
                .Include(w => w.RoleFkNavigation)
                .FirstOrDefaultAsync(m => m.UserPk == id);
            if (worksUser == null)
            {
                return NotFound();
            }

            return View(worksUser);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleFk"] = new SelectList(_context.UserRole, "UserRolePk", "UserRolePk");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserPk,FirstName,LastName,Address,City,State,Zip,Country,Email,Phone,Login,Password,RoleFk")] WorksUser worksUser)
        {
            if (ModelState.IsValid)
            {
                var aUser = await _context.WorksUser.FirstOrDefaultAsync(u => u.Login == worksUser.Login);
                if (aUser is null)
                {
                    _context.Add(worksUser);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    //TempData["success"] = "Account created";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["message"] = "Choose a different Login name";
                }

            }
            ViewData["RoleFk"] = new SelectList(_context.UserRole, "UserRolePk", "UserRolePk", worksUser.RoleFk);
            return View(worksUser);
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            int userPK = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (userPK == 0)
            {
                return NotFound();
            }
            var worksUser = await _context.WorksUser.FindAsync(userPK);

            if (worksUser == null)
            {
                return NotFound();
            }
            ViewData["RoleFk"] = new SelectList(_context.UserRole, "UserRolePk", "UserRolePk", worksUser.RoleFk);
            return View(worksUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserPk,FirstName,LastName,Address,City,State,Zip,Country,Email,Phone,Login,Password,RoleFk")] WorksUser worksUser)
        {
            int id = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != worksUser.UserPk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worksUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorksUserExists(worksUser.UserPk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            ViewData["RoleFk"] = new SelectList(_context.UserRole, "UserRolePk", "UserRolePk", worksUser.RoleFk);
            return View(worksUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksUser = await _context.WorksUser
                .Include(w => w.RoleFkNavigation)
                .FirstOrDefaultAsync(m => m.UserPk == id);
            if (worksUser == null)
            {
                return NotFound();
            }

            return View(worksUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worksUser = await _context.WorksUser.FindAsync(id);
            _context.WorksUser.Remove(worksUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserOrderDetail", "Users");
        }

        private bool WorksUserExists(int id)
        {
            return _context.WorksUser.Any(e => e.UserPk == id);
        }
    }
}