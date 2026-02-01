using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public int ItemId { get; set; }
        public DateTime DateSold { get; set; }
        public int QtySold { get; set; }
        public string? ChannelType { get; set; }
        public int? BoothId { get; set; }
        public decimal UnitSalePrice { get; set; }
        public decimal? DiscountedRate { get; set; }
    }

}
