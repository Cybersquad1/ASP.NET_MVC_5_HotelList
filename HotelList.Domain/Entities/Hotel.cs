using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelList.Domain.Entities
{
    public class Hotel
    {
        public int HotelID { get; set; }
        public string NameHotel { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
    }
}
