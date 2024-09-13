using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CheckoutOrderCommandHandler(IOrderRepository orderRepo, ILogger<CheckoutOrderCommandHandler> logger, IMapper mapper)
        {
             _orderRepository = orderRepo;
            _logger = logger;
            _mapper = mapper;   
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Order>(request);
            var generatedOrder = await _orderRepository.AddAsync(entity);
            _logger.LogInformation($"--> Order {generatedOrder.Id} successfully created!");
            return generatedOrder.Id;
            
        }
    }
}
