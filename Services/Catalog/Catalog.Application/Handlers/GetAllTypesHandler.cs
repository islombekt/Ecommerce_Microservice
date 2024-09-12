using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypesRepository _types;
        public GetAllTypesQueryHandler(ITypesRepository types)
        {
            _types = types;
        }

        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typesList = await _types.GetAllTypes();
            var typesResponse =ProductMapper.Mapper.Map<IList<TypesResponse>>(typesList);
            return typesResponse;
        }
    }
}
