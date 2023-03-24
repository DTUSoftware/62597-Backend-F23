using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using ShopBackend.Repositories;


namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productRepository.GetAll();

            if (products != null && products.Count() > 0)
            {
                return Ok(products);
            }

            return NotFound("Products do not existed!");
        }

        // GET: api/Products/{5}
        [HttpGet("{productId}", Name = "GetProductById")]
        public async Task<ActionResult<Product?>> Get(string productId)
        {
            var product = await _productRepository.Get(productId);

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound("The specified product does not exist!");
        }

        // Post: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            if (product.Id == null)
            {
                return BadRequest("ProductId cannot be null");
            }

            var existed = await _productRepository.Get(product.Id);
            if (existed != null)
            {
                return BadRequest("ProductId is already in the databse.");
            }

            var result = await _productRepository.Insert(product);
            if (result != default && result > 0)
            {
                var location = Url.Action(nameof(Get), new { id = product.Id }) ?? $"/{product.Id}";
                return Created(location, product.Id + " is inserted successfully.");
            }

            return NotFound("Product cannot be inserted");
        }



        // Put: api/Products/5
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] Product product)
        {
            if (product.Id == null)
            {
                return BadRequest("ProductId cannot be null");
            }

            var existed = await _productRepository.Get(product.Id);
            if (existed == null)
            {
                return NotFound("Product cannot be found");
            }

            var result = await _productRepository.Update(product);
            if (result != default && result > 0)
            {
                return Ok("Product is updated successfully");
            }

            return NotFound("Product cannot be updated!");
        }


        // Delete: api/Products/5
        [HttpDelete("{productId}")]
        public async Task<ActionResult<string>> Delete(string productId)
        {
            if (productId == null)
            {
                return BadRequest("ProductId cannot be null");
            }
            var existed = await _productRepository.Get(productId);
            if (existed == null)
            {
                return NotFound("product cannot be found.");
            }

            var result = await _productRepository.Delete(productId);
            if (result != default)
            {
                return Ok("product is deleted successfully.");
            }

            return NotFound("Product cannot be deleted!");
        }
    }
}
