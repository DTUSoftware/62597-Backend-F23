using ShopBackend.Dtos;
using ShopBackend.Models;
using System.Diagnostics.Metrics;
using System.Net;

namespace ShopBackend
{
    public static class Extensions
    {
        public static CustomerDto AsCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Address = customer.Address != null ? new List<AddressDto>(customer.Address.Select(x => x.AsAddressDto())) : new List<AddressDto>(),
                Orders = customer.Orders != null ? new List<OrderDto>(customer.Orders.Select(x => x.AsOrderDto())) : new List<OrderDto>(),
            };
        }

        public static AddressDto AsAddressDto(this Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                StreetAddress = address.StreetAddress,
                Type = address.Type,
            };
        }

        public static OrderDto AsOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderDetails = order.OrderDetails != null ? new List<OrderDetailDto>(order.OrderDetails.Select(x => x.AsOrderDetailDto())) : new List<OrderDetailDto>(),
            };
        }

        public static OrderDetailDto AsOrderDetailDto(this OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                Quantity = orderDetail.Quantity,
                Product = orderDetail.Product!.AsProductDto(),
            };
        }

        public static ProductDto AsProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Currency = product.Currency,
                RebateQuantity = product.RebateQuantity,
                RebatePercent = product.RebatePercent,
                UpsellProductId = product.UpsellProductId,
            };
        }





        public static Customer AsCustomerModel(this CustomerDto customerDto)
        {
            return new Customer
            {
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Phone = customerDto.Phone,
                Address = customerDto.Address != null ? new List<Address>(customerDto.Address.Select(x => x.AsAddressModel())) : new List<Address>(),
                Orders = customerDto.Orders != null ? new List<Order>(customerDto.Orders.Select(x => x.AsOrderModel())) : new List<Order>(),
            };
        }

        public static Address AsAddressModel(this AddressDto addressDto)
        {
            return new Address
            {
                Id = addressDto.Id,
                ZipCode = addressDto.ZipCode,
                Country = addressDto.Country,
                City = addressDto.City,
                StreetAddress = addressDto.StreetAddress,
                Type = addressDto.Type,
            };
        }

        public static Order AsOrderModel(this OrderDto orderDto)
        {
            return new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                OrderStatus = orderDto.OrderStatus,
                OrderDetails = orderDto.OrderDetails != null ? new List<OrderDetail>(orderDto.OrderDetails.Select(x => x.AsOrderDetailModel())) : null,
            };
        }

        public static OrderDetail AsOrderDetailModel(this OrderDetailDto orderDetailDto)
        {
            return new OrderDetail
            {
                Id = orderDetailDto.Id,
                Quantity = orderDetailDto.Quantity,
                Product = orderDetailDto.Product?.AsProductModel()
            };
        }

        public static Product AsProductModel(this ProductDto productDto)
        {
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price,
                Currency = productDto.Currency,
                RebateQuantity = productDto.RebateQuantity,
                RebatePercent = productDto.RebatePercent,
                UpsellProductId = productDto.UpsellProductId,
            };
        }






        public static Order CreateAsOrderModel(this CreateUpdateOrderDto orderDto)
        {
            return new Order
            {
                OrderDate = orderDto.OrderDate,
                OrderStatus = orderDto.OrderStatus,
                CustomerEmail = orderDto.CustomerEmail,
            };
        }

        public static OrderDetail CreateAsOrderDetailModel(this CreateOrderDetailDto orderDetailDto)
        {
            return new OrderDetail
            {
                Quantity = orderDetailDto.Quantity,
                OrderId = orderDetailDto.OrderId,
                ProductId = orderDetailDto.ProductId,
            };
        }

        public static Customer CreateAsCustomerModel(this CreateCustomerDto customerDto)
        {
            return new Customer
            {
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Password= customerDto.Password,
                Phone = customerDto.Phone,
            };
        }

        public static Address CreateAsAddressModel(this CreateAddressDto addressDto)
        {
            return new Address
            {
                ZipCode = addressDto.ZipCode,
                Country = addressDto.Country,
                City = addressDto.City,
                StreetAddress = addressDto.StreetAddress,
                Type = addressDto.Type,
                CustomerEmail = addressDto.CustomerEmail,
            };
        }
    }
}
