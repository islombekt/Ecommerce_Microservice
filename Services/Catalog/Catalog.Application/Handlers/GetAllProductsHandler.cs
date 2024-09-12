using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _prod;
        public GetAllProductQueryHandler(IProductRepository product)
        {
            _prod = product;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var prodList = await _prod.GetProducts(request.CatalogSpecParams);
            var prodResponce = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(prodList); // better for lists, no need to make foreach property use .ForMember(...)
            return prodResponce;
        }
    }
}
