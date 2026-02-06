using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class AuctionProfitDto
    {
        public int AcqId { get; set; }
        public string Title { get; set; }
        public int? LotNumber { get; set; }
        public string AuctionSite { get; set; }
        public decimal TotalSettlement { get; set; }
        public decimal SuggestedBoothPrice { get; set; }
        public decimal PotentialProfit { get; set; }
    }

}
