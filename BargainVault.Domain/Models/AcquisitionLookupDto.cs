using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class AcquisitionLookupDto
    {
        public int AcqId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
    }

}
