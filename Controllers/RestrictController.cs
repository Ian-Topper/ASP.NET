

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//add the following namespaces

using Microsoft.EntityFrameworkCore;
using DroneWorks.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DroneWorks.Controllers
{
    public class RestrictController : Controller
    {
        private readonly Team116DBContext _context;

        public RestrictController(Team116DBContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            // retrieve the user's PK from the Claims collection
            // since the PK is stored as a string, it has to be parsed to an integer

            int userPK = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            // retrieve the user's orders

            var orderDetail = _context.WorksOrder
                .Include(od => od.UserFkNavigation)
                .Include(od => od.OrderItem)
                .Include(od => od.UserFk)
                .Include(od => od.OrderDate)
                .Include(od => od.ShipAddress)
                .Include(od => od.ShipCity)
                .Include(od => od.ShipState)
                .Include(od => od.ShipZip)
                .Include(od => od.ShipCountry)
                .Include(od => od.ShipDate)
                .Include(od => od.OrderStatus)
                .Include(od => od.UserFkNavigation)
                .Where(u => u.UserFkNavigation.UserPk == userPK)
                .OrderBy(d => d.OrderDate);

            return View(await orderDetail.ToListAsync());
        }

        [Authorize]
        public IActionResult CheckOut()
        {
            return RedirectToAction("MyCart", "Shop");
        }

        [Authorize]
        public IActionResult PlaceOrder()
        {
            Cart aCart = GetCart();

            if (aCart.CartItems().Any())
            {
                int userPK = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
                string streetAddress = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.StreetAddress).Value;
                string shipCity = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Locality).Value;
                string shipState = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.StateOrProvince).Value;
                string shipZip = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PostalCode).Value;
                string shipCountry = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Country).Value;



                // insert order
                Random egg = new Random();
                int pk = egg.Next();
            
                WorksOrder aOrder = new WorksOrder { OrderPk = pk, UserFk = userPK, OrderDate = DateTime.Now, ShipAddress = streetAddress,
                ShipCity = shipCity, ShipState = shipState, ShipZip =Convert.ToInt32(shipZip), ShipDate =null, ShipCountry = shipCountry, OrderStatus = "recieved" };

                _context.Add(aOrder);
                _context.SaveChanges();

                // get the PK of the newly inserted order

                int orderPK = aOrder.OrderPk;

                // insert a orderdetail for each item in the cart

                foreach (CartItem aItem in aCart.CartItems())
                {
                    OrderItem aDetail = new OrderItem { ProdFk = aItem.Product.ProdPk, Quantity = aItem.Quantity, OrderFk = orderPK };
                    _context.Add(aDetail);
                }

                _context.SaveChanges();

                // remove all items from cart

                aCart.ClearCart();

                SaveCart(aCart);

                return View(nameof(OrderConfirmation));
            }

            return RedirectToAction("Search", "Shop");
        }

        private IActionResult OrderConfirmation()
        {
            return View();
        }

        private Cart GetCart()
        {
            Cart aCart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return aCart;
        }

        private void SaveCart(Cart aCart)
        {
            HttpContext.Session.SetObject("Cart", aCart);
        }
    }
}