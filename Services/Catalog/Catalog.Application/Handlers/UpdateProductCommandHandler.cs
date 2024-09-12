using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler : IReqestHandler<UpdateProductCommand,bool>
    {
        private readonly IProductRepository _product;
        public UpdateProductCommandHandler(IProductRepository product)
        {
            _product = product;
        }
        public async Task<bool> Handle(UpdateProductCommand command,CancellationToken cancellation)
        {
            var prEntity = await _product.UpdateProduct(new Core.Entities.Product
            {
                Id = command.Id,
                Description = command.Description,
                Brands = command.Brands,
                Name = command.Name,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Summary = command.Summary,
                Types = command.Types
            });
            return prEntity;
        }
    }
}
