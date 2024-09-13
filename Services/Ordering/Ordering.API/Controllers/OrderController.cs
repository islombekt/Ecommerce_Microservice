using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;

namespace Ordering.API.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IMediator mediator, ILogger<OrderController> logger) 
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet("GetOrders/{userName}")]
        public async Task<IActionResult> GetOrders(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CheckoutOrderCommand command)
        {
          
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var cmd = new DeleteOrderCommand() { Id=id};
            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
