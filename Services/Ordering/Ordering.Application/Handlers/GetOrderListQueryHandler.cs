

using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public GetOrderListQueryHandler(IOrderRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _orderRepo = repo;
        }
        public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var resp = await _orderRepo.GetOrdersByUserName(request.UserName);
            var result = _mapper.Map<List<OrderResponse>>(resp);
            return result;
        }
    }
}
