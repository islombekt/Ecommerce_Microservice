using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;
        public BasketOrderingConsumer(ILogger<BasketOrderingConsumer> logger, IMapper mapper,IMediator mediator)
        {
            _mediator= mediator;
            _mapper= mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
           using var scope = _logger.BeginScope("Consuming basket Checkout Event for {correlationId}",context.Message.CorrelationId);
           var cmd = _mapper.Map<CheckoutOrderCommand>(context.Message);
           var res = await _mediator.Send(cmd);
           _logger.LogInformation("Basket Checkout Event compeleted!!");

        }
    }
}
