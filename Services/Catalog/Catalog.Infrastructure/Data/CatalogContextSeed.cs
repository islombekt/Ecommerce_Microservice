using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> catalogCollection)
        {
            try { 
            bool checkProduct = catalogCollection.Find(b => true).Any();
            string path = Path.Combine("..", "app", "Data", "SeedData", "products.json");
            if (!checkProduct)
            {
                var productsData = File.ReadAllText(path);
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products != null)
                {
                    foreach (var item in products)
                    {
                        catalogCollection.InsertOneAsync(item);
                    }
                }
            }
            }
            catch { }
        }
    }
}
