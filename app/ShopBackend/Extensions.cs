using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend
{
    public static class Extensions
    {
        public static CustomerDto AsCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                Email = customer.Email,
                Role = customer.Role,
                Orders = customer.Orders != null ? new List<OrderDto>(customer.Orders.Select(x => x.AsOrderDto())) : new List<OrderDto>(),
            };
        }

        public static AddressDto AsAddressDto(this Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Email = address.Email,
                MobileNr = address.MobileNr,
                Company = address.Company,
                VatNr = address.VatNr,
                Country = address.Country,
                ZipCode = address.ZipCode,
                City = address.City,
                Address1 = address.Address1,
                Address2 = address.Address2,
            };
        }

        public static CreateAddressDto AsCreateAddressDto(this Address address)
        {
            return new CreateAddressDto
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Email = address.Email,
                MobileNr = address.MobileNr,
                Company = address.Company,
                VatNr = address.VatNr,
                Country = address.Country,
                ZipCode = address.ZipCode,
                City = address.City,
                Address1 = address.Address1,
                Address2 = address.Address2,
            };
        }

        public static OrderDto AsOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                CheckMarketing = order.CheckMarketing,
                SubmitComment = order.SubmitComment,
                BillingAddress = order.BillingAddress.AsAddressDto(),
                ShippingAddress = order.ShippingAddress.AsAddressDto(),
                OrderDetails = new List<OrderDetailDto>(order.OrderDetails.Select(x => x.AsOrderDetailDto())),
            };
        }

        public static OrderDetailDto AsOrderDetailDto(this OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                Quantity = orderDetail.Quantity,
                GiftWrap = orderDetail.GiftWrap,
                RecurringOrder = orderDetail.RecurringOrder,
                Product = orderDetail.Product.AsProductDto(),
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
                ImageUrl = product.ImageUrl,
            };
        }





        public static Customer AsCustomerModel(this CustomerDto customerDto)
        {
            return new Customer
            {
                Email = customerDto.Email,
                Role = customerDto.Role,
                Orders = customerDto.Orders != null ? new List<Order>(customerDto.Orders.Select(x => x.AsOrderModel())) : new List<Order>(),
            };
        }

        public static Address AsAddressModel(this AddressDto addressDto)
        {
            return new Address
            {
                Id = addressDto.Id,
                FirstName = addressDto.FirstName,
                LastName = addressDto.LastName,
                Email = addressDto.Email,
                MobileNr = addressDto.MobileNr,
                Company = addressDto.Company,
                VatNr = addressDto.VatNr,
                Country = addressDto.Country,
                ZipCode = addressDto.ZipCode,
                City = addressDto.City,
                Address1 = addressDto.Address1,
                Address2 = addressDto.Address2,
            };
        }

        public static Order AsOrderModel(this OrderDto orderDto)
        {
            return new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                OrderStatus = orderDto.OrderStatus,
                CheckMarketing = orderDto.CheckMarketing,
                SubmitComment = orderDto.SubmitComment,
                BillingAddress = orderDto.BillingAddress.AsAddressModel(),
                ShippingAddress = orderDto.ShippingAddress.AsAddressModel(),
                OrderDetails = new List<OrderDetail>(orderDto.OrderDetails.Select(x => x.AsOrderDetailModel()))
            };
        }

        public static OrderDetail AsOrderDetailModel(this OrderDetailDto orderDetailDto)
        {
            return new OrderDetail
            {
                Id = orderDetailDto.Id,
                Quantity = orderDetailDto.Quantity,
                GiftWrap = orderDetailDto.GiftWrap,
                RecurringOrder = orderDetailDto.RecurringOrder,
                Product = orderDetailDto.Product.AsProductModel()
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
                ImageUrl = productDto.ImageUrl,
            };
        }






        public static Order CreateAsOrderModel(this CreateOrderDto orderDto)
        {

            return new Order
            {
                Id = orderDto.Id,
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                CheckMarketing = orderDto.CheckMarketing,
                SubmitComment = orderDto.SubmitComment,
                CustomerEmail = orderDto.CustomerEmail,
                ShippingAddress = orderDto.ShippingAddress.CreateAsAddressModel(),
                BillingAddress = orderDto.BillingAddress.CreateAsAddressModel(),
                OrderDetails = new List<OrderDetail>(orderDto.OrderDetails.Select(x => x.CreateAsOrderDetailModel()))
            };
        }

        public static OrderDetail CreateAsOrderDetailModel(this CreateOrderDetailDto orderDetailDto)
        {
            return new OrderDetail
            {
                Quantity = orderDetailDto.Quantity,
                GiftWrap = orderDetailDto.GiftWrap,
                RecurringOrder = orderDetailDto.RecurringOrder,
                OrderId = orderDetailDto.OrderId,
                ProductId = orderDetailDto.ProductId,
            };
        }

        public static Address CreateAsAddressModel(this CreateAddressDto addressDto)
        {
            return new Address
            {
                FirstName = addressDto.FirstName,
                LastName = addressDto.LastName,
                Email = addressDto.Email,
                MobileNr = addressDto.MobileNr,
                Company = addressDto.Company,
                VatNr = addressDto.VatNr,
                Country = addressDto.Country,
                ZipCode = addressDto.ZipCode,
                City = addressDto.City,
                Address1 = addressDto.Address1,
                Address2 = addressDto.Address2
            };
        }

        public static Product CreateAsProductModel(this CreateProductDto productDto)
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
                ImageUrl = productDto.ImageUrl,
            };
        }
    }
}
