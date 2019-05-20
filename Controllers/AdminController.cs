using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DroneWorks.Models;
using Microsoft.AspNetCore.Authorization;

namespace DroneWorks.Controllers
{ [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Team116DBContext _context;

        public AdminController(Team116DBContext context)
        {
            _context = context;
        }

        // GET: Admin
        //public async Task<IActionResult> Index()
        //{
        //    var team116DBContext = _context.Products.Include(p => p.CatFkNavigation);
        //    return View(await team116DBContext.ToListAsync());
        //}

        public IActionResult Index(string searchName, decimal? priceMin, decimal? priceMax)
        {
            ViewData["NameFilter"] = searchName;
            ViewData["PriceMinFilter"] = priceMin;
            ViewData["PriceMaxFilter"] = priceMax;

            var products = from p in _context.Products select p;

            if (!String.IsNullOrEmpty(searchName))
            {
                products = products.Where(p => p.ProdName.Contains(searchName));
            }

            if (priceMin != null)
            {
                products = products.Where(p => p.ProdPrice >= priceMin);
            }

            if (priceMax != null)
            {
                products = products.Where(p => p.ProdPrice <= priceMax);
            }

            return View(products.OrderBy(p => p.ProdName).ThenBy(p => p.ProdPrice).ToList());
        }


        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.ProdPk == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdPk,ProdName,ProdDescribe,ProdPrice,ImageName,CatFk,TotalStock")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["message"] = "You Have Created a New Product!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk", products.CatFk);
            return View(products);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk", products.CatFk);
            return View(products);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdPk,ProdName,ProdDescribe,ProdPrice,ImageName,CatFk,TotalStock")] Products products)
        {
            if (id != products.ProdPk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "The Product Has Been Updated!!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProdPk))
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
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk", products.CatFk);
            return View(products);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.ProdPk == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            TempData["message"] = "The Product Has Been Deleted!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProdPk == id);
        }
    }
}
