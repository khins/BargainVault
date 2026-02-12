using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class FacebookPostDto
    {
        public int? PostId { get; set; }
        public int AcqId { get; set; }

        public DateTime? PostDate { get; set; }
        public string? PostTitle { get; set; }
        public string? PostDescription { get; set; }

        public decimal? AskingPrice { get; set; }
        public bool Boosted { get; set; }
        public bool MarkAsSold { get; set; }
        public DateTime? RenewDate { get; set; }
        public bool IsEditMode { get; set; }
    }

}
