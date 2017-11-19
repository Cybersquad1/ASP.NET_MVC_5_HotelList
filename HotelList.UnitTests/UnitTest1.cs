using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HotelList.Domain.Abstract;
using HotelList.Domain.Entities;
using HotelList.WebUI.Controllers;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using HotelList.WebUI.Models;
using HotelList.WebUI.HtmlHelpers;

namespace HotelList.UnitTests
{
    [TestClass]
    public class UnitTest1

    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[]
            {
                new Hotel {HotelID = 1, NameHotel ="P1" },
                new Hotel {HotelID = 2, NameHotel ="P2" },
                new Hotel {HotelID = 3, NameHotel ="P3" },
                new Hotel {HotelID = 4, NameHotel ="P4" },
                new Hotel {HotelID = 5, NameHotel ="P5" },
            });

            HotelController controller = new HotelController(mock.Object);
            controller.PageSize = 3;

            HotelListViewModel result = (HotelListViewModel)controller.List(null, 2).Model;

            Hotel[] prodArray = result.Hotels.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].NameHotel, "P4");
            Assert.AreEqual(prodArray[1].NameHotel, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>"
            + @"<a class=""btn btn-default btn primary selected"" href=""Strona2"">2</a>"
            + @"<a class =""btn btn-default"" href=""Strona3"">3</a>",
            result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[]
            {
                new Hotel {HotelID = 1, NameHotel ="P1" },
                new Hotel {HotelID = 2, NameHotel ="P2" },
                new Hotel {HotelID = 3, NameHotel ="P3" },
                new Hotel {HotelID = 4, NameHotel ="P4" },
                new Hotel {HotelID = 5, NameHotel ="P5" },
            });

            HotelController controller = new HotelController(mock.Object);
            controller.PageSize = 3;

            HotelListViewModel result = (HotelListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[] {
                new Hotel { HotelID = 1, NameHotel = "P1", City = "City1" },
                new Hotel { HotelID = 2, NameHotel = "P2", City = "City2" },
                new Hotel { HotelID = 3, NameHotel = "P3", City = "City3" },
                new Hotel { HotelID = 4, NameHotel = "P4", City = "City4" },
                new Hotel { HotelID = 5, NameHotel = "P5", City = "City5" },
            });

            HotelController controller = new HotelController(mock.Object);
            controller.PageSize = 3;

            Hotel[] result = ((HotelListViewModel)controller.List("Cat2", 1).Model).Hotels.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].NameHotel == "P2" && result[0].City == "City2");
            Assert.IsTrue(result[1].NameHotel == "P4" && result[1].City == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[]
            {
                new Hotel {HotelID = 1, NameHotel="P1", City="Jabłka"},
                new Hotel {HotelID = 2, NameHotel="P2", City="Jabłka"},
                new Hotel {HotelID = 3, NameHotel="P3", City="Śliwki"},
                new Hotel {HotelID = 4, NameHotel="P4", City="Pomarańcze"},
            });

            NavController target = new NavController(mock.Object);

            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Jabłka");
            Assert.AreEqual(results[1], "Śliwki");
            Assert.AreEqual(results[2], "Pomarańcze");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[]
            {
                new Hotel {HotelID = 1, NameHotel = "P1", City = "Jabłka"},
                new Hotel {HotelID = 2, NameHotel = "P2", City = "Pomarańcze"}
            });

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Jabłka";

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[] {
            new Hotel {HotelID = 1, NameHotel = "P1", City = "City1"},
            new Hotel {HotelID = 2, NameHotel = "P2", City = "City2"},
            new Hotel {HotelID = 3, NameHotel = "P3", City = "City3"},
            new Hotel {HotelID = 4, NameHotel = "P4", City = "City4"},
            new Hotel {HotelID = 5, NameHotel = "P5", City = "City5"}
            });
            // przygotowanie — tworzenie kontrolera i ustawienie 3-elementowej strony
            HotelController target = new HotelController(mock.Object);
            target.PageSize = 3;
            // działanie — testowanie liczby produktów dla różnych kategorii
            int res1 = ((HotelListViewModel)target.List("City1").Model).PagingInfo.TotalItems;
            int res2 = ((HotelListViewModel)target.List("City2").Model).PagingInfo.TotalItems;
            int res3 = ((HotelListViewModel)target.List("City3").Model).PagingInfo.TotalItems;
            int resAll = ((HotelListViewModel)target.List(null).Model).PagingInfo.TotalItems;
            // asercje
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Hotel p1 = new Hotel { HotelID = 1, NameHotel = "P1" };
            Hotel p2 = new Hotel { HotelID = 2, NameHotel = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Hotel, p1);
            Assert.AreEqual(results[1].Hotel, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Hotel p1 = new Hotel { HotelID = 1, NameHotel = "P1" };
            Hotel p2 = new Hotel { HotelID = 2, NameHotel = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Hotel.HotelID).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Hotel p1 = new Hotel { HotelID = 1, NameHotel = "P1" };
            Hotel p2 = new Hotel { HotelID = 2, NameHotel = "P2" };
            Hotel p3 = new Hotel { HotelID = 3, NameHotel = "P3" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            Assert.AreEqual(target.Lines.Where(c => c.Hotel == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);

        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Hotel p1 = new Hotel { HotelID = 1, NameHotel = "P1", Price = 100M };
            Hotel p2 = new Hotel { HotelID = 2, NameHotel = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Hotel p1 = new Hotel { HotelID = 1, NameHotel = "P1", Price = 100M };
            Hotel p2 = new Hotel { HotelID = 2, NameHotel = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[] {
            new Hotel {HotelID = 1, NameHotel = "P1", City = "Jab"},
            }.AsQueryable());

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);
            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Hotel.HotelID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {

            Mock<IHotelRepository> mock = new Mock<IHotelRepository>();
            mock.Setup(m => m.Hotels).Returns(new Hotel[] {
            new Hotel {HotelID = 1, NameHotel = "P1", City = "Jabłka"},
            }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null);
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {

            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
            Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Hotel(), 1);
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
            Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Hotel(), 1);
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
            Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
