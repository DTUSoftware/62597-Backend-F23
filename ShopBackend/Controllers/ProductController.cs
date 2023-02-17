using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;

namespace ShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = new List<Product> {
                new Product
                {
                    Id = "vitamin-d-90-100",
                    Name = "D-vitamin, 90ug, 100 stk",
                    Price = 116,
                    Currency = "DKK",
                    RebateQuantity = 3,
                    RebatePercent = 10,
                    UpsellProductId = null
                }
            };

            return Ok(products);
        }
    }
}
