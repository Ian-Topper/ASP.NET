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
{
    public class ProductController : Controller
    {
        private readonly Team116DBContext _context;

        public ProductController(Team116DBContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var team116DBContext = _context.Products.Include(p => p.CatFkNavigation);
            return View(await team116DBContext.ToListAsync());
        }

        // GET: Product/Details/5
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

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk");
            return View();
        }

        // POST: Product/Create
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatFk"] = new SelectList(_context.Category, "CatPk", "CatPk", products.CatFk);
            return View(products);
        }

        // GET: Product/Edit/5
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

        // POST: Product/Edit/5
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

        // GET: Product/Delete/5
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

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProdPk == id);
        }
        //public IActionResult TableView()
        //{
        //    //lambda expression - write methods in a shorthand way(can use to access database)
        //    var products = _context.Products.Include(p => p.CatFkNavigation);

        //    return View(products.ToList());
        //}

        //public IActionResult ListView()
        //{
        //    var products = _context.Products.OrderBy(p => p.ProdName);

        //    return View(products.ToList());
        //}

        public IActionResult SortView(string sortOrder)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "priceDesc" : "price";

            var products = from p in _context.Products select p;

            switch (sortOrder)
            {
                case "nameDesc":
                    products = products.OrderByDescending(p => p.ProdName);
                    break;
                case "price":
                    products = products.OrderBy(p => p.ProdPrice);
                    break;
                case "priceDesc":
                    products = products.OrderByDescending(p => p.ProdPrice);
                    break;
                default:
                    products = products.OrderBy(p => p.ProdName);
                    break;
            }

            return View(products.ToList());
        }

        public IActionResult FilterView(string searchName, decimal? priceMin, decimal? priceMax)
        {
            ViewData["NameFilter"] = searchName;
            ViewData["PriceMinFilter"] = priceMin;
            ViewData["PriceMaxFilter"] = priceMax;

            var products = from p in _context.Products select p;

            if (!string.IsNullOrEmpty(searchName))
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
    }

}
