using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using System.Xml.Linq;


namespace ShopBackend.Controllers
{
    [Route("api/[ordercontroller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Order> orders = new List<Order> {
                new Order
                {
                    id = "Order1",
                    total = 100
                },
                new Order
                {
                    id = "Order2",
                    total = 200
                }
        };
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return Ok(orders)
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Order>> GetSingleOrder(string id)
        {
            var order = orders.Find(x => x.Id == id);
            if (order is null)
            {
                return NotFound("Order does not exist");
            }
            return Ok(order);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(Order order)
        {
            orders.Add(order);
            return Ok(order);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(string id, Order orderData)
        {
            var order = products.Find(x => x.id == id);
            if (order is null)
            {
                return NotFound("The order does not exist");
            }
            order.total = orderData.total;
            //TODO Do this in a better way
            order.products.clear();
            foreach (ProductController p in orderData.products)
            {
                order.products.Add(p);
            }
            return Ok(order);
        }
        HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            var order = orders.Find(x => x.id == id);
            if (order is null)
            {
                return NotFound("The order does not exist");
            }
            orders.Remove(order);
            return Ok(order);
        }
    }
}