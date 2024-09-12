using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepo;
        public GetDiscountQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepo = discountRepository;
        }
        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var data = await _discountRepo.GetDiscount(request.ProductName);
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            var resp = new CouponModel
            {
                Id = data.Id,
                Description = data.Description,
                Amount = data.Amount,
                ProductName = data.ProductName,
            };
            return resp;
           
        }
    }
}
