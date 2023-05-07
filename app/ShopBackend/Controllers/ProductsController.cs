using Microsoft.AspNetCore.Mvc;
using ShopBackend.Repositories;
using ShopBackend.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using ShopBackend.Discoverabillity;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        //private readonly LinkGenerator _linkGenerator;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            //_linkGenerator = linkGenerator;
        }


        // GET: api/products
        [HttpGet("all/{page}")]
        [AllowAnonymous]
        [EnableCors("FrontendPolicy")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get(int page)
        {
            var products = (await _productRepository.GetAll(page)).Select(product => product.AsProductDto());
            if (products.Any())
            {
                var productList = products.ToList();
                foreach (ProductDto prod in productList)
                {
                    prod.Links = (List<Link>)CreateLinksForProduct(prod.Id,"GET");
                }

                return Ok(productList);
            }

            return NotFound("The specified products does not exist!");
        }


        // GET: api/products/{productId}
        [HttpGet("{productId}")]
        [AllowAnonymous]
        [EnableCors("FrontendPolicy")]
        public async Task<ActionResult<ProductDto>> Get(string productId)
        {
            var product = await _productRepository.Get(productId);
            if (product != default)
            {
                ProductDto prod = product.AsProductDto();
                prod.Links = (List<Link>)CreateLinksForProduct(productId, "GET");
                return Ok(prod);
            }

            return NotFound("The specified product does not exist!");
        }


        // Post: api/products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Create([FromBody] CreateProductDto product)
        {
            if (string.IsNullOrEmpty(product.Id))
            {
                return BadRequest("Product ID is required to create the product!");
            }

            var isIdTaken = await _productRepository.Get(product.Id);
            if (isIdTaken != default)
            {
                return BadRequest("This product id is already in use!");
            }

            var result = await _productRepository.Insert(product.CreateAsProductModel());
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForProduct(product.Id,"POST"));
            }

            return NotFound("Product could not be inserted!");
        }

        // Post: api/products/multiple
        [HttpPost("multiple")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> CreateMultiple(IEnumerable<CreateProductDto> products)
        {
            foreach (CreateProductDto product in products)
            {
                var result = await _productRepository.Insert(product.CreateAsProductModel());
                if (result == default || result == 0)
                {
                    return NotFound($"Product {product.Name} could not be inserted!");
                }
            }

            return Ok("Products were inserted successfully!");
        }


        // Put: api/products
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Update([FromBody] CreateProductDto product)
        {
            if (string.IsNullOrEmpty(product.Id))
            {
                return BadRequest("Product ID is required to update the product!");
            }
            var productToUpdate = await _productRepository.Get(product.Id);
            if (productToUpdate == default)
            {
                return NotFound("Product does not exist!");
            }

            productToUpdate.Id = product.Id;
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Currency = product.Currency;
            productToUpdate.RebateQuantity = product.RebateQuantity;
            productToUpdate.RebatePercent = product.RebatePercent;
            productToUpdate.UpsellProductId = product.UpsellProductId;
            productToUpdate.ImageUrl = product.ImageUrl;

            var result = await _productRepository.Update(productToUpdate);
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForProduct(product.Id, "PUT"));
            }

            return BadRequest("Product could not be updated!");
        }


        // Delete: api/products/{productId}
        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Delete(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID is required to delete the product!");
            }
            var result = await _productRepository.Delete(productId);
            if (result != default)
            {
                return Ok("Product has been deleted!");
            }

            return NotFound("Product could not be deleted!");
        }

        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForProduct(string productId, string requestType)
        {
            /*
            var GetUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { productId})!
            var DeleteUrl= _linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { productId })!
            var UpdateUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Update))!
           */
            var GetUrl = HttpContext + nameof(Get) + new { productId };
            var DeleteUrl = HttpContext + nameof(Delete) + new { productId };
            var UpdateUrl = HttpContext + nameof(Update);

            switch (requestType)
            {
                case "GET":
                    return new List<Link> {
                        new Link(href: DeleteUrl, "delete_product", "DELETE"),
                        new Link(href: UpdateUrl, "update_product", "PUT")
                    };
                case "PUT":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_product", "DELETE")
                    };
                case "POST":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_product", "DELETE"),
                        new Link(href: UpdateUrl, "update_product", "PUT")
                    };
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}
