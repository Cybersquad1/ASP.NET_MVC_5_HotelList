using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelList.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Hotel hotel, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Hotel.HotelID == hotel.HotelID)
                .FirstOrDefault();

            if(line == null)
            {
                lineCollection.Add(new CartLine { Hotel = hotel, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Hotel hotel)
        {
            lineCollection.RemoveAll(l => l.Hotel.HotelID == hotel.HotelID); 
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Hotel.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    public class CartLine
    {
        public Hotel Hotel { get; set; }
        public int Quantity { get; set; }
    }
}
