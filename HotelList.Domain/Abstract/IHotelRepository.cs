using System.Collections.Generic;
using HotelList.Domain.Entities;
namespace HotelList.Domain.Abstract
{
    public interface IHotelRepository
    {
        IEnumerable<Hotel> Hotels { get; }
    }
}
