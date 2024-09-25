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
    public static class TypesContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            try
            {
                bool checkTypes = typeCollection.Find(b => true).Any();
                string path = Path.Combine("..", "app", "Data", "SeedData", "types.json");
                if (!checkTypes)
                {
                    var typesData = File.ReadAllText(path);
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types != null)
                    {
                        foreach (var item in types)
                        {
                            typeCollection.InsertOneAsync(item);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
