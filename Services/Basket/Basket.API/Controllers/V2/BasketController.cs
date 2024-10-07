using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [ApiController]
    [Route("api/v2/[controller]")]
    public class BasketController : ControllerBase
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
        [HttpPost("[action]")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            // get the existing basket with username
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null) { return BadRequest(); }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);
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
