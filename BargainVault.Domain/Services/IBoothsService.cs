using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IBoothsService
    {
        Task<List<BoothDto>> GetBoothsAsync();
    }

}
