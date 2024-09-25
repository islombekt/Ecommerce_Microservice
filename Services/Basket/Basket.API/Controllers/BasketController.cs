using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publish;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IMediator mediator, IPublishEndpoint publish, ILogger<BasketController> logger)
        {
            _logger = logger;
            _publish = publish;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get the existing basket with username
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null) { return BadRequest(); }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            try
            {
                await _publish.Publish(eventMsg);
                _logger.LogInformation($"Basket published for {basket.UserName}");
                // remove the basket

                var deleteCmd = new DeleteShoppingCartByUserNameCommand(basket.UserName);
                await _mediator.Send(deleteCmd);

                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
