using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.ProductType.Find(c => true).ToListAsync();
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands.Find(d => true).ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deletePr = await _context.Products.DeleteOneAsync(d => d.Id == id);
            return deletePr.IsAcknowledged && deletePr.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams CatalogSpecParams)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(CatalogSpecParams.Search))
            {
                filter = filter & Builders<Product>.Filter.Where(d => d.Name.ToLower().Contains(CatalogSpecParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(CatalogSpecParams.BrandId))
            {

                filter = filter & Builders<Product>.Filter.Eq(d => d.Brands.Id, CatalogSpecParams.BrandId);
            }
            if (!string.IsNullOrEmpty(CatalogSpecParams.TypeId))
            {

                filter = filter & Builders<Product>.Filter.Eq(d => d.Types.Id, CatalogSpecParams.BrandId);
            }
            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(CatalogSpecParams, filter);
            // await _context.Products.Find(filter).Skip((CatalogSpecParams.PageIndex-1)*CatalogSpecParams.PageSize)
            //.Limit(CatalogSpecParams.PageSize).ToListAsync();

            return new Pagination<Product>(
                CatalogSpecParams.PageIndex, CatalogSpecParams.PageSize, (int)totalItems, data
                    );
        }



        public async Task<IEnumerable<Product>> GetProductsByBrand(string brand)
        {
            return await _context.Products.Find(d => d.Brands.Name.ToLower() == brand.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Find(d => d.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatePr = await _context.Products.ReplaceOneAsync(d => d.Id == product.Id, product);
            return updatePr.IsAcknowledged && updatePr.ModifiedCount > 0;
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefn = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        break;

                }
            }
            return await _context.Products
                        .Find(filter)
                        .Sort(sortDefn)
                        .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                        .Limit(catalogSpecParams.PageSize).ToListAsync();
        }
    }
}
