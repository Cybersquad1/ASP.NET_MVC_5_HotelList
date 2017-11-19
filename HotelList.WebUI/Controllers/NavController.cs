using System.Collections.Generic;
using System.Web.Mvc;
using HotelList.Domain.Abstract;
using System.Linq;

namespace HotelList.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IHotelRepository repository;

        public NavController(IHotelRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Hotels
                .Select(x => x.City)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}