﻿using Microsoft.AspNetCore.Mvc;
using UserReviewsWebsiteAPI.Database.Models;
using UserReviewsWebsiteAPI.Database.Models.Dtos;
using UserReviewsWebsiteAPI.Services;

namespace UserReviewsWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            IEnumerable<Product> product = _service.GetProducts();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public ActionResult GetProduct(int id)
        {
            Product product = _service.GetProduct(id);
            return Ok(product);
        }


        [HttpPost]
        public ActionResult AddProduct(ProductDto product)
        {
            _service.AddProduct(product);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, ProductDto product)
        {
            _service.UpdateProduct(id, product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            _service.DeleteProduct(id);
            return Ok();
        }
    }
}
