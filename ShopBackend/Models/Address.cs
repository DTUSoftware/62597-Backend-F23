﻿using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? MobileNr { get; set; }

        public string? Company { get; set; }

        public string? VatNr { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }

        public string? City { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }



        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
