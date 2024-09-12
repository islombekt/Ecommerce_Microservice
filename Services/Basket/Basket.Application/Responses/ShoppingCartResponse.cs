using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse : ShoppingCart
    {
        public string UserName { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;  
        public List<ShoppingCartItemResponse> Items { get; set; } = new List<ShoppingCartItemResponse>();

        public ShoppingCartResponse()
        { }
        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public decimal TotalPrice { get { return Items.Sum(c=>c.Price*c.Quantity); } }
        public decimal TQ { get { return Items.Sum(c => c.Quantity); } }
    }
}
