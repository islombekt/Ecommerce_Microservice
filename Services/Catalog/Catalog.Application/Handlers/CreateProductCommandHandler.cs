using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class CreateProductCommandHandler : IReqestHandler<CreateProductCommand,ProductResponse>
    {
        private readonly IProductRepository _repository;
        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken cancel)
        {
            var newPr = ProductMapper.Mapper.Map<Product>(command);
            if (newPr is null) { 
                throw new ArgumentNullException("Issues while creating mapping new product!!");
            }
            var prEntity = await _repository.CreateProduct(newPr);
            var returnPr = ProductMapper.Mapper.Map<ProductResponse>(prEntity);
            return returnPr; 
        }
    }
}
