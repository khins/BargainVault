using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class FacebookPostListDto
    {
        public int PostId { get; set; }
        public int AcqId { get; set; }

        public string ItemTitle { get; set; } = string.Empty;
        public DateTime? PostDate { get; set; }

        public decimal? AskingPrice { get; set; }
        public bool Boosted { get; set; }
        public bool MarkAsSold { get; set; }
    }

}
