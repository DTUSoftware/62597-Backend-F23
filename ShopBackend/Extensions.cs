using Microsoft.AspNetCore.Http;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Utils;
using System.Collections.Generic;

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

        public static OrderDto AsOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                CheckMarketing = order.CheckMarketing,
                SubmitComment = order.SubmitComment,
                Addresses = order.Addresses != null ? new List<AddressDto>(order.Addresses.Select(x => x.AsAddressDto())) : new List<AddressDto>(),
                OrderDetails = order.OrderDetails != null ? new List<OrderDetailDto>(order.OrderDetails.Select(x => x.AsOrderDetailDto())) : new List<OrderDetailDto>(),
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
                ImageUrl = product.ImageUrl,
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
                Addresses = orderDto.Addresses != null ? new List<Address>(orderDto.Addresses.Select(x => x.AsAddressModel())) : null,
                OrderDetails = orderDto.OrderDetails != null ? new List<OrderDetail>(orderDto.OrderDetails.Select(x => x.AsOrderDetailModel())) : null,
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
                ImageUrl = productDto.ImageUrl,
            };
        }






        public static Order CreateAsOrderModel(this CreateOrderDto orderDto)
        {
            var addressList = new List<Address>();
            if (orderDto.BillingAddress != null)
            {
                orderDto.BillingAddress.IsBillingAddress = true;
                orderDto.BillingAddress.IsShippingAddress = false;
                addressList.Add(orderDto.BillingAddress.CreateAsAddressModel());
            }
            if (orderDto.ShippingAddress != null)
            {
                orderDto.ShippingAddress.IsBillingAddress = false;
                orderDto.ShippingAddress.IsShippingAddress = true;
                addressList.Add(orderDto.ShippingAddress.CreateAsAddressModel());
            }

            return new Order
            {
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                CheckMarketing = orderDto.CheckMarketing,
                SubmitComment = orderDto.SubmitComment,
                CustomerEmail = orderDto.CustomerEmail,
                Addresses = addressList,
                OrderDetails = orderDto.OrderDetails != null ? new List<OrderDetail>(orderDto.OrderDetails.Select(x => x.CreateAsOrderDetailModel())) : null,
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
                IsBillingAddress = addressDto.IsBillingAddress,
                IsShippingAddress= addressDto.IsShippingAddress,
            };
        }
    }
}
