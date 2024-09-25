using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    public class CatalogController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;   
        public CatalogController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById (string productId)
        {
            var query = new GetProductByIdQuery(productId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetProductByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var query = new GetProductByNameQuery(name);
            var result = await _mediator.Send(query);
            _logger.LogInformation($"Product with {name} fetched");
            return Ok(result);
        }
        [HttpGet("GetProductByBrand/{brand}")]
        public async Task<IActionResult> GetProductByBrand(string brand)
        {
            var query = new GetProductByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            try
            {
                var query = new GetAllProductQuery(catalogSpecParams);
                var result = await _mediator.Send(query);
               // _logger.LogInformation("All products retrieved");
                return Ok(result);
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "An Exception has occured: {Exception}");
                throw;
            }
        }
        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductCommand command)
        {
            var result = await _mediator.Send<ProductResponse>(command);
            return Ok(result);
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var result = await _mediator.Send<bool>(command);
            return Ok(result);
        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var query = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send<bool>(query);
            return Ok(result);
        }
    }
}
