using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelList.Domain.Entities;

namespace HotelList.WebUI.Models
{
    public class HotelListViewModel
    {
        public IEnumerable<Hotel> Hotels { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}