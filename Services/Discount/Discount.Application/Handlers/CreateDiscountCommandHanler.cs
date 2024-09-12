using AutoMapper;
using Discount.Application.Commands;
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
    public class CreateDiscountCommandHanler : IRequestHandler<CreateCouponCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepo;
        private readonly IMapper _mapper;
        public CreateDiscountCommandHanler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _mapper = mapper;
            _discountRepo = discountRepository;
        }
        public async Task<CouponModel> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var couponCreated = await _discountRepo.CreateDiscount(new Core.Entities.Coupon { Amount = request.Amount,Description=request.Description,ProductName=request.ProductName });
            var couponModel = _mapper.Map<CouponModel>(couponCreated);
            return couponModel;
        }
    }
}
