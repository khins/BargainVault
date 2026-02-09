using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class RetailPriceDto
    {
        public int RetailPriceId { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public decimal RetailPrice { get; set; }
        public DateTime? PriceDate { get; set; }
        public bool IsSalePrice { get; set; }
        public string? Notes { get; set; }
    }

}
