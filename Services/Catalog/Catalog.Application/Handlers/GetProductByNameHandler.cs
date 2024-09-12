using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetProductByNameQueryHandler : IReqestHandler<GetProductByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository productRepository;
        public GetProductByNameQueryHandler(IProductRepository pr)
        {
            productRepository = pr;
        }
        public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery query, IList<ProductResponse> response)
        {
            var product = await productRepository.GetProductsByName(query.ProductName);
            var productResp = ProductMapper.Mapper.Map<IList<ProductResponse>>(product);
            return productResp;
        }
    }
}
