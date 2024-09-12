using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
        {
            _orderRepo = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var find = await _orderRepo.GetByIdAsync(request.Id);
            if (find == null) {
                throw new OrderNotFountException(nameof(Order), request.Id);
            }
            _mapper.Map(request, find, typeof(UpdateOrderCommand),typeof(Order));
            await _orderRepo.UpdateAsync(find);
            _logger.LogInformation($"--> Order by {request.Id} is successfully updated!");
            return Unit.Value;
        }
    }
}
