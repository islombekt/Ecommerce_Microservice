
using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService
{
    public class DiscountGrpcService 
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountPServ;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountPServ = discountProtoService;
        }
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountReq = new GetDiscountRequest { ProductName = productName };
            return await _discountPServ.GetDiscountAsync(discountReq);
        }
    }
}
