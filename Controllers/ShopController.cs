
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//Add the following namespaces

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DroneWorks.Models;
using Microsoft.AspNetCore.Authorization;

namespace DroneWorks.Controllers
{
    public class ShopController : Controller
    {
        private readonly Team116DBContext _context;
        public ShopController(Team116DBContext context)
        {
            _context = context;
        }

        public IActionResult Search(string searchName, decimal? priceMin, decimal? priceMax, string sortOrder)
        {
            // the ViewData elements are used pass back the filter values to the FilterDemo View

            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "priceDesc" : "price";
            ViewData["NameFilter"] = searchName;
            ViewData["PriceMinFilter"] = priceMin;
            ViewData["PriceMaxFilter"] = priceMax;
            

            var products = from p in _context.Products select p;

            // depending on the filter values (received as parameters from the query string in the URL), where methods are used to filter the IQueryable object, products

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

            

            //switch (sortOrder)
            //{
            //    case "nameDesc":
            //        products = products.OrderByDescending(p => p.ProdName);
            //        break;
            //    case "price":
            //        products = products.OrderBy(p => p.ProdPrice);
            //        break;
            //    case "priceDesc":
            //        products = products.OrderByDescending(p => p.ProdPrice);
            //        break;
            //    default:
            //        products = products.OrderBy(p => p.ProdName);
            //        break;

            //}

            return View(products.OrderBy(p => p.ProdName).ThenBy(p => p.ProdPrice).ToList());
             
        }
        // prepare output to display details for a product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Search));
            }

            var products = await _context.Products
                .Include(p => p.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.ProdPk == id);

            if (products == null)
            {
                return RedirectToAction(nameof(Search));
            }

            return View(products);
        }

        // add a product to shopping cart
        public IActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Search));
            }

            var products = _context.Products.FirstOrDefault(p => p.ProdPk == id);

            if (products == null)
            {
                return RedirectToAction(nameof(Search));
            }

            // call to method to retrieve cart object from session state

            Cart aCart = GetCart();

            aCart.AddItem(products);

            // call to method to save cart object to session state

            SaveCart(aCart);

            return RedirectToAction(nameof(MyCart));
        }

        // prepare output to display items in cart object
        [Authorize]
        public IActionResult MyCart()
        {
            Cart aCart = GetCart();

            if (aCart.CartItems().Any())
            {
                return View(aCart);
            }

            // if the cart is empty

            return RedirectToAction(nameof(Search));
        }
        [Authorize]
        // update cart - i.e., the quantity for a product in the cart
        public IActionResult UpdateCart(int? productPK, int qty)
        {
            if (productPK == null)
            {
                return RedirectToAction(nameof(Search));
            }

            var product = _context.Products.FirstOrDefault(p => p.ProdPk == productPK);

            if (product == null)
            {
                return RedirectToAction(nameof(Search));
            }

            Cart aCart = GetCart();

            aCart.UpdateItem(product, qty);

            SaveCart(aCart);

            return RedirectToAction(nameof(MyCart));
        }

        // remove an item from the cart
        public IActionResult RemoveFromCart(int? productPK)
        {
            if (productPK == null)
            {
                return RedirectToAction(nameof(Search));
            }

            var product = _context.Products.FirstOrDefault(p => p.ProdPk == productPK);

            if (product == null)
            {
                return RedirectToAction(nameof(Search));
            }

            Cart aCart = GetCart();

            aCart.RemoveItem(product);

            SaveCart(aCart);

            return RedirectToAction(nameof(MyCart));
        }

        //method to retrieve cart object from session state
        private Cart GetCart()
        {
            // call the session extension method GetObject
            // if a cart object doesn't exist, create a new cart object

            Cart aCart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return aCart;
        }

        //method to save cart object to session state
        private void SaveCart(Cart aCart)
        {
            // call the session extension method SetObject

            HttpContext.Session.SetObject("Cart", aCart);
        }
    }
}