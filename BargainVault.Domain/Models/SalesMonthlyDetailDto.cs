using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class SalesMonthlyDetailDto
    {
        public int SaleId { get; set; }
        public DateTime DateSold { get; set; }
        public string ItemTitle { get; set; } = string.Empty;
        public int QtySold { get; set; }
        public decimal UnitSalePrice { get; set; }
        public decimal LineTotal => QtySold * UnitSalePrice;
        public string? ChannelType { get; set; }
        public string? BoothName { get; set; }
    }

}
