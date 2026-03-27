using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class LiquidationCandidateDto
    {
        public int ItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal? TotalSettlement { get; set; }
        public decimal? AuctionEstimate { get; set; }
        public string Recommendation { get; set; } = string.Empty;
    }

}
