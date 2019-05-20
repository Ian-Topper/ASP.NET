using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DroneWorks.Models;

namespace DroneWorks.Controllers
{
    public class WorksOrders1Controller : Controller
    {
        private readonly Team116DBContext _context;

        public WorksOrders1Controller(Team116DBContext context)
        {
            _context = context;
        }

        // GET: WorksOrders1
        public async Task<IActionResult> Index()
        {
            var team116DBContext = _context.WorksOrder.Include(w => w.UserFkNavigation);
            return View(await team116DBContext.ToListAsync());
        }

        // GET: WorksOrders1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksOrder = await _context.WorksOrder
                .Include(w => w.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderPk == id);
            if (worksOrder == null)
            {
                return NotFound();
            }

            return View(worksOrder);
        }

        // GET: WorksOrders1/Create
        public IActionResult Create()
        {
            ViewData["UserFk"] = new SelectList(_context.WorksUser, "UserPk", "UserPk");
            return View();
        }

        // POST: WorksOrders1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderPk,UserFk,OrderDate,ShipAddress,ShipCity,ShipState,ShipZip,ShipCountry,ShipDate,OrderStatus")] WorksOrder worksOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worksOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFk"] = new SelectList(_context.WorksUser, "UserPk", "UserPk", worksOrder.UserFk);
            return View(worksOrder);
        }

        // GET: WorksOrders1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksOrder = await _context.WorksOrder.FindAsync(id);
            if (worksOrder == null)
            {
                return NotFound();
            }
            ViewData["UserFk"] = new SelectList(_context.WorksUser, "UserPk", "UserPk", worksOrder.UserFk);
            return View(worksOrder);
        }

        // POST: WorksOrders1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderPk,UserFk,OrderDate,ShipAddress,ShipCity,ShipState,ShipZip,ShipCountry,ShipDate,OrderStatus")] WorksOrder worksOrder)
        {
            if (id != worksOrder.OrderPk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worksOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorksOrderExists(worksOrder.OrderPk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFk"] = new SelectList(_context.WorksUser, "UserPk", "UserPk", worksOrder.UserFk);
            return View(worksOrder);
        }

        // GET: WorksOrders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksOrder = await _context.WorksOrder
                .Include(w => w.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderPk == id);
            if (worksOrder == null)
            {
                return NotFound();
            }

            return View(worksOrder);
        }

        // POST: WorksOrders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worksOrder = await _context.WorksOrder.FindAsync(id);
            _context.WorksOrder.Remove(worksOrder);
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            TempData["success"] = "You Have Successfully Deleted your Order!!!";
            return RedirectToAction("UserOrderDetail", "Users");
        }

        private bool WorksOrderExists(int id)
        {
            return _context.WorksOrder.Any(e => e.OrderPk == id);
        }
    }
}
