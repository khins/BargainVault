using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IFacebookPostsService
    {
        Task<int> InsertFacebookPostAsync(FacebookPostDto dto, string enteredBy);
        Task UpdateFacebookPostAsync(FacebookPostDto dto, string enteredBy);
        Task DeleteFacebookPostAsync(int postId, string enteredBy);

        Task<FacebookPostDto?> GetFacebookPostByIdAsync(int postId);
        Task<List<FacebookPostListDto>> GetFacebookPostsAsync();
    }

}
