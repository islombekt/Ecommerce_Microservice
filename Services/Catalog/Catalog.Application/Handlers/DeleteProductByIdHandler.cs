using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class DeleteProductByIdHandler : IReqestHandler<DeleteProductByIdCommand,bool>
    {
        private readonly IProductRepository _prRepo;
        public DeleteProductByIdHandler(IProductRepository product)
        {
            _prRepo=product;
        }
        public async Task<bool> Handle(DeleteProductByIdCommand command,CancellationToken cancellation)
        {
            var delete = await _prRepo.DeleteProduct(command.Id);
            return delete;
        }   
    }
}
