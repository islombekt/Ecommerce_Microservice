
using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null) { 
                return new Coupon() { ProductName="No Discount", Amount=0,};
            }
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affecterd = await connection.ExecuteAsync(
                "insert into Coupon (ProductName, Description, Amount) values (@ProductName, @Description,@Amount)", new
                {
                    ProductName = coupon.ProductName,
                    Amount = coupon.Amount,
                    Description = coupon.Description,
                }
                );
            if(affecterd == 0) return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affecterd = await connection.ExecuteAsync(
                "Delete from Coupon where ProductName = @ProductName ", new
                {
                   
                    ProductName = productName
                  
                }
                );
            if (affecterd == 0) return false;
            return true;
        }

       
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affecterd = await connection.ExecuteAsync(
                "update Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount where Id = @Id ", new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Amount = coupon.Amount,
                    Description = coupon.Description,
                }
                );
            if (affecterd == 0) return false;
            return true;
        }
    }
}
