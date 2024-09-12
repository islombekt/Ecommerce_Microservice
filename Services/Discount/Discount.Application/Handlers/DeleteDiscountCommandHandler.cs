

using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepo;
        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            _discountRepo = discountRepository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _discountRepo.DeleteDiscount(request.ProductName);
            return deleted;
        }
    }
}
