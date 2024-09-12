using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Extension
{
    public static class InfraServices
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection servicesCollection, IConfiguration configuration)
        { 
            servicesCollection.AddDbContext<OrderContext>(
                options => options.UseSqlServer(configuration.GetConnectionString(
                        "OrderingConnectionString")));
            servicesCollection.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            servicesCollection.AddScoped<IOrderRepository, OrderRepository>();
            return servicesCollection;
        }
    }
}
