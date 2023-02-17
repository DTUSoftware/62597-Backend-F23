using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using System.Xml.Linq;

namespace ShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product> {
                new Product
                {
                    Id = "vitamin-d-90-100",
                    Name = "D-vitamin, 90ug, 100 stk",
                    Price = 116,
                    Currency = "DKK",
                    RebateQuantity = 3,
                    RebatePercent = 10,
                    UpsellProductId = null
                },
                new Product
                {
                    Id = "vitamin-d-90-101",
                    Name = "D-vitamin, 90ug, 50 stk",
                    Price = 14,
                    Currency = "DKK",
                    RebateQuantity = 2,
                    RebatePercent = 34,
                    UpsellProductId = null
                }
        };

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetSingleProduct(string id)
        {
            var product = products.Find(x => x.Id == id);
            if (product is null)
            {
                return NotFound("The specified product does not exist!");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            products.Add(product);
            return Ok(product);
        }

        [HttpPost("Multiple")]
        public async Task<ActionResult<List<Product>>> AddMultipleProducts(List<Product> newProducts)
        {
            foreach (Product p in newProducts)
            {
                products.Add(p);
            }
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(string id, Product data)
        {
            var product = products.Find(x => x.Id == id);
            if (product is null)
            {
                return NotFound("The specified product does not exist!");
            }

            product.Name = data.Name;
            product.Price = data.Price;
            product.Currency = data.Currency;
            product.RebateQuantity = data.RebateQuantity;
            product.RebatePercent = data.RebatePercent;
            product.UpsellProductId = data.UpsellProductId;

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var product = products.Find(x => x.Id == id);
            if (product is null)
            {
                return NotFound("The specified product does not exist!");
            }
            products.Remove(product);
            return Ok(product);
        }

    }
}
