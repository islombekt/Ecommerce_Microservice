using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetOrders());
                await context.SaveChangesAsync();
                logger.LogInformation($"--> Ordering Database: {typeof(OrderContext).Name} seeded");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
        {
            new()
            {
                UserName = "islom",
                FirstName = "tokhirov",
                LastName = "Zokir ogli",
                EmailAddress = "islombektakhirov@gmail.com",
                AddressLine = "Tashkent",
                Country = "Uzbekistan",
                TotalPrice = 750,
                State = "",
                ZipCode = "22222222",

                CardName = "Visa",
                CardNumber = "0000000000000000",
                CreatedBy = "Islom",
                Expiration = "12/25",
                Cvv = "001",
                PaymentMethod = 1,
                LastModifiedBy = "Islom",
                LastModifiedDate = new DateTime(),
            }
        };
        }
    }
}
