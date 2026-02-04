using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class InventoryLocationListDto
    {
        public int InventoryLocationId { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = string.Empty;
        public string? BoothName { get; set; }
        public string? StatusName { get; set; }
        public DateTime? DatePlaced { get; set; }
        public decimal? AskingPrice { get; set; }
    }
}
