using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers
{
    public class GetBasketByUserNameHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepo;
      
        public GetBasketByUserNameHandler(IBasketRepository basketRepository)
        {
            _basketRepo = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var resp = await _basketRepo.GetBasket(request.UserName);
            var ShoppingResp = BasketMapper.Mapper.Map<ShoppingCartResponse>(resp);
            return ShoppingResp;
                    
        }
    }
}
