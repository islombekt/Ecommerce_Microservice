using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
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
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepo;
        private readonly DiscountGrpcService _discountService;
        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountGrpcService discount)
        {
            _basketRepo = basketRepository;
            _discountService = discount;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items) 
            {
                var coupon = await _discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            var shoppingCar = await _basketRepo.UpdateBasket(new Core.Entities.ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items,
            });
            var response = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCar);
            return response;
        }
    }
}
