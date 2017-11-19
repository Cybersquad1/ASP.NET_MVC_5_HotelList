using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelList.Domain.Abstract;
using HotelList.Domain.Concrete;
using HotelList.Domain.Entities;

namespace HotelList.Domain.Concrete
{
    public class EFHotelRepository : IHotelRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<Hotel> Hotels
        {
            get { return context.Hotels; }
        }
    }
}
