using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class AcquisitionListDto
    {
        public int AcqId { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public DateTime? DateAcquired { get; set; }
        public decimal? TotalSettlement { get; set; }
        public string StatusName { get; set; }
        public bool Personal { get; set; }
        public bool BusinessExpense { get; set; }
    }
}
