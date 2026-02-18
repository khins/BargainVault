using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class RetailPriceListDto
    {
        public int RetailPriceId { get; set; }

        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = "";

        public int StoreId { get; set; }
        public string StoreName { get; set; } = "";

        public decimal RetailPrice { get; set; }
        public DateTime? PriceDate { get; set; }

        public bool IsSalePrice { get; set; }
    }
}

