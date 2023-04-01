using Microsoft.AspNetCore.Mvc;
using ShopBackend.Repositories;
using ShopBackend.Dtos;

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
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var products = (await _productRepository.GetAll()).Select(product => product.AsProductDto());
            if (products.Any())
            {
                return Ok(products);
            }

            return NotFound("The specified products does not exist!");
        }


        // GET: api/Products/{5}
        [HttpGet("{productId}", Name ="GetProductById")]
        public async Task<ActionResult<ProductDto>> Get(string productId)
        {
            var product = await _productRepository.Get(productId);
            if(product != default)
            {
                return Ok(product.AsProductDto()); 
            }

            return NotFound("The specified product does not exist!");
        }


        // Post: api/Products
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] ProductDto product)
        {
            if (product.Id == null)
            {
                return BadRequest("Product id is required to register the product!");
            }
            var isIdTaken = await _productRepository.Get(product.Id);
            if (isIdTaken != default)
            {
                return BadRequest("This product id is already in use!");
            }

            var result = await _productRepository.Insert(product.AsProductModel());
            if(result != default && result > 0) 
            { 
                return Ok("Product is inserted successfully!"); 
            }

            return NotFound("Product could not be inserted!");
        }

        // Post: api/Products/Multiple Primarily used for populating the server database
        [HttpPost("Multiple")]
        public async Task<ActionResult<string>> CreateMultiple(IEnumerable<ProductDto> products)
        {
            foreach (ProductDto product in products)
            {
                var result = await _productRepository.Insert(product.AsProductModel());
                if (result == default || result == 0)
                {
                    return NotFound($"Product {product.Name} could not be inserted!");
                }
            }

            return Ok("Product is inserted successfully!");
        }


        // Put: api/Products/5
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] ProductDto product)
        {
            var productToUpdate = await _productRepository.Get(product.Id);
            if (productToUpdate == default)
            {
                return BadRequest("Product does not exsist!");
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Currency = product.Currency;
            productToUpdate.RebateQuantity = product.RebateQuantity;
            productToUpdate.RebatePercent = product.RebatePercent;
            productToUpdate.UpsellProductId = product.UpsellProductId;

            var result = await _productRepository.Update(productToUpdate);
            if(result != default && result > 0)
            { 
                return Ok("Product updated successfully!"); 
            }

            return NotFound("Product could not be updated!");
        }


        // Delete: api/Products/5
        [HttpDelete("{productId}")]
        public async Task<ActionResult<string>> Delete(string productId)
        {
            var result=await _productRepository.Delete(productId);
            if(result !=default)
            {
                return Ok("Product has been deleted!"); 
            }

            return NotFound("Product could not be deleted!");
        }
    }
}
