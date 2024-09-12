using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;
     
        public BasketController(IMediator mediator)
        {
          
            _mediator = mediator;
        }
        [HttpGet("GetBasketByName/{userName}")]
        public async Task<IActionResult> GetProductById(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("CreateBasket")]
        public async Task<IActionResult> CreateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {
           
            var result = await _mediator.Send(createShoppingCartCommand);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            var query = new DeleteShoppingCartByUserNameCommand(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
