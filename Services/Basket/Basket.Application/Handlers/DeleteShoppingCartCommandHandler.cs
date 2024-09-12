
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class DeleteShoppingCartCommandHandler : IRequestHandler<DeleteShoppingCartByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _basketRepo;
        public DeleteShoppingCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepo = basketRepository;
        }
        public async Task<Unit> Handle(DeleteShoppingCartByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _basketRepo.DeleteBasket(request.UserName);
            return Unit.Value;
        }
    }
}
