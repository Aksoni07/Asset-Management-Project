using AssetManagement.Core;
using AssetManagement.Core.Entities;

namespace AssetManagement.BusinessLogic.Services
{
    public interface IAssetService
    {
        // CRUD Methods
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset> GetAssetByIdAsync(int id);
        Task AddAssetAsync(Asset asset);
        Task UpdateAssetAsync(Asset asset);
        Task DeleteAssetAsync(int id);
        
        // Business Logic
        Task AssignAssetAsync(int assetId, int employeeId, string notes);
        Task<IEnumerable<Asset>> GetAvailableAssetsAsync();

        // Search Method
        Task<PagedResult<Asset>> SearchAssetsAsync(
            string? serialNumber, 
            string? status, 
            string? assetType, 
            int? employeeId, 
            int pageNumber, 
            int pageSize, 
            string sortBy, 
            bool isAscending);
            
        // Report Methods
        Task<IEnumerable<AssetAssignmentHistory>> GetAssignmentHistoryAsync(int assetId);
        Task<IEnumerable<Asset>> GetAssetsNearingWarrantyExpiryAsync(int days);
        Task ReturnAssetAsync(int assetId);
    }
}