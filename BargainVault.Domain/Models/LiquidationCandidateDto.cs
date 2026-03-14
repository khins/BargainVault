using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class LiquidationCandidateDto
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImagePath { get; set; }
        public bool Disregard { get; set; }

        public int? AcqId { get; set; }
        public decimal? TotalSettlement { get; set; }
        public DateTime? DateAcquired { get; set; }

        public decimal? RetailPrice { get; set; }

        public DateTime? LastPostDate { get; set; }
        public decimal? LastAskingPrice { get; set; }

        public int DaysInInventory { get; set; }

        public decimal AuctionPct { get; set; }
        public decimal? AuctionEstimate { get; set; }

        public int IleScore { get; set; }
        public string Recommendation { get; set; }
    }

}
