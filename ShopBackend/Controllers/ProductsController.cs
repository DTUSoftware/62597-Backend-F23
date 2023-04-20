using Microsoft.AspNetCore.Mvc;
using ShopBackend.Repositories;
using ShopBackend.Dtos;
using Microsoft.AspNetCore.Routing;
using ShopBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly LinkGenerator _linkGenerator;

        public ProductsController(IProductRepository productRepository, LinkGenerator linkGenerator)
        {
            _productRepository = productRepository;
            _linkGenerator = linkGenerator;
        }


        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var products = (await _productRepository.GetAll()).Select(product => product.AsProductDto());
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


        // GET: api/Products/{5}
        [HttpGet("{productId}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(string productId)
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
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForProduct(product.Id,"POST"));
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
        [HttpPut("{productId}", Name = "UpdateProductById")]
        public async Task<ActionResult<string>> UpdateProductById([FromBody] ProductDto product)
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
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForProduct(product.Id, "PUT"));
            }

            return NotFound("Product could not be updated!");
        }


        // Delete: api/Products/5
        [HttpDelete("{productId}", Name = "DeleteProductById")]
        public async Task<ActionResult<string>> DeleteProductById(string productId)
        {
            var result = await _productRepository.Delete(productId);
            if (result != default)
            {
                return Ok("Product has been deleted!");
            }

            return NotFound("Product could not be deleted!");
        }
        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForProduct(String productId, String requestType)
        {
            switch (requestType)
            {
                case "GET":
                    var linksGet = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteProductById), values: new { productId }),
            "delete_product",
            "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateProductById), values: new { productId }),
        "update_product",
        "PUT")
            };
            return linksGet;
                case "PUT":
                    var linksPut = new List<Link>
                        {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetProductById), values: new { productId}),
            "self",
            "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteProductById), values: new { productId }),
            "delete_product",
            "DELETE")
            };
                    return linksPut;



                case "POST":
                    var linksPost = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetProductById), values: new { productId}),
            "self",
            "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteProductById), values: new { productId }),
            "delete_product",
            "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateProductById), values: new { productId }),
        "update_product",
        "PUT")
            };
                    return linksPost;

                default:
                    throw new Exception("Invalid requestType");
            }


        }

    }

}
