﻿using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products{
            get;
        }

        public IMongoCollection<ProductBrand> Brands
        {
            get;
        }

        public IMongoCollection<ProductType> ProductType
        {
            get;
        }
        public CatalogContext(IConfiguration configuration) { 
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Brands = database.GetCollection<ProductBrand>(configuration.GetValue<string>("DatabaseSettings:BrandsCollection"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            ProductType = database.GetCollection<ProductType>(configuration.GetValue<string>("DatabaseSettings:TypesCollection"));
            BrandContextSeed.SeedData(Brands);
           TypesContextSeed.SeedData(ProductType);
           CatalogContextSeed.SeedData(Products);
        }
    }
}
