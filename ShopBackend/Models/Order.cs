﻿using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public Enum? OrderStatus { get; set; }

        public Address? BillingAddress { get; set; }

        public Address? ShippingAddress { get; set; }

        public bool CheckMarketing { get; set; }

        public string? SubmitComment { get; set; }



        public string? CustomerEmail { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
