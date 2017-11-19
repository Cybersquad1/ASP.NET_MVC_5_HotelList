using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelList.Domain.Abstract;
using HotelList.Domain.Entities;
using HotelList.WebUI.Models;

namespace HotelList.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IHotelRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IHotelRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }
            public CartController(IHotelRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                ReturnUrl = returnUrl,
                Cart = cart
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int hotelId, string returnUrl)
        {
            Hotel hotel = repository.Hotels
            .FirstOrDefault(p => p.HotelID == hotelId);
            if (hotel != null)
            {
                cart.AddItem(hotel, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int hotelId, string returnUrl)
        {
            Hotel hotel = repository.Hotels
            .FirstOrDefault(p => p.HotelID == hotelId);
            if (hotel != null)
            {
                cart.RemoveLine(hotel);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if(cart == null) {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Rezerwacja jest pusta!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}