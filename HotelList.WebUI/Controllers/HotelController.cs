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
    public class HotelController : Controller
    {
        private IHotelRepository repository;
        public int PageSize = 4;
        public HotelController(IHotelRepository hotelRepository)
        {
            this.repository = hotelRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            HotelListViewModel viewModel = new HotelListViewModel
            {
                Hotels = repository.Hotels
                .Where(p => category == null || p.City == category)
                .OrderBy(p => p.HotelID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        repository.Hotels.Count() : 
                        repository.Hotels.Where(e => e.City == category).Count()
                },
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}