using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;


namespace Catalog.Application.Handlers
{
    public class GetProductByBrandQueryHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _prod;
        public GetProductByBrandQueryHandler(IProductRepository prod) { _prod = prod; }

        public async Task<IList<ProductResponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellation)
        {
            var prod = await _prod.GetProductsByBrand(request.brand);
            var prodResp = ProductMapper.Mapper.Map<IList<ProductResponse>>(prod);
            return prodResp;
        }

    }
}
