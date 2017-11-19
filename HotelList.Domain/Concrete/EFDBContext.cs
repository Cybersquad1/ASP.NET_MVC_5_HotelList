using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HotelList.Domain.Entities;
using HotelList.Domain.Abstract;

namespace HotelList.Domain.Concrete
{
    class EFDBContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
    }
}
