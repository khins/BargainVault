using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class InventoryLocationDto
    {
        public int InventoryLocationId { get; set; }
        public int ItemId { get; set; }
        public int? BoothId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DatePlaced { get; set; }
        public decimal? AskingPrice { get; set; }
        public string? Notes { get; set; }
    }
}
