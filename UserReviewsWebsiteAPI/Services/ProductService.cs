﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserReviewsWebsiteAPI.Database.Data;
using UserReviewsWebsiteAPI.Database.Models;
using UserReviewsWebsiteAPI.Database.Models.Dtos;
using UserReviewsWebsiteAPI.Dtos;
using UserReviewsWebsiteAPI.Exceptions;
using UserReviewsWebsiteAPI.Interfaces;

namespace UserReviewsWebsiteAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PageResult<Product>> GetProducts(ProductQuery query)
        {
            var baseQuery = _db.Products.Include(p => p.Reviews).Where(p => query.SearchPhrase == null ||
            p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var colummsSelector = new Dictionary<string, Expression<Func<Product, object>>>
                {
                    {nameof(Product.Name), r => r.Name },
                    {"Score", r => r.Reviews.Average(r => r.Score) }
                };

                var selectedColumn = colummsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var products = baseQuery.Skip(query.PageSize * (query.PageNumber -1)).Take(query.PageSize).ToList();

            if(products == null)
            {
                throw new NotFoundException("Products not found");
            }

            var totalItemsCount = baseQuery.Count();

            var result = new PageResult<Product>(products, totalItemsCount, query.PageSize, query.PageNumber);

            if(result.ItemsTo > result.TotalItemsCount)
            {
                result.ItemsTo = result.TotalItemsCount;
            }

            if (result.ItemsFrom > result.TotalItemsCount)
            {
                result.ItemsFrom = result.TotalItemsCount;
            }

            return result;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(r => r.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            return product;
        }

        public async Task<Product> AddProduct(ProductDto createProduct)
        {
            var product = new Product
            {

                Name = createProduct.Name,
                Description = createProduct.Description,
                ImagePath = createProduct.ImagePath
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return product;

        }

        public async Task<Product> UpdateProduct(int id, ProductDto updateProduct)
        {
            var product = _db.Products.FirstOrDefault(r => r.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.ImagePath = updateProduct.ImagePath;

            await _db.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(r => r.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return product;
        }
    }
}
