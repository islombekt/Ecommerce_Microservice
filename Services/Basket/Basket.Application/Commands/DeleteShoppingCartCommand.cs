

using MediatR;

namespace Basket.Application.Commands
{
    public class DeleteShoppingCartByUserNameCommand : IRequest<Unit>
    {
        public string UserName { get; set; }
        public DeleteShoppingCartByUserNameCommand(string userName)
        {
            UserName = userName;
        }
    }
}
