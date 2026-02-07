using BargainVault.Domain.Models.BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class GlobalSearchResultDto
    {
        public GlobalSearchEntityType EntityType { get; set; }

        public int EntityId { get; set; }

        public string DisplayText { get; set; } = string.Empty;

        public string? SecondaryText { get; set; }
    }

}
