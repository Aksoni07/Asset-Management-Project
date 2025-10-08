using AssetManagement.Core.Entities;

namespace AssetManagement.BusinessLogic.Services
{
    public interface IAssetService
    {
        // CRUD for Assets
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset> GetAssetByIdAsync(int id);
        Task AddAssetAsync(Asset asset);
        Task UpdateAssetAsync(Asset asset);
        Task DeleteAssetAsync(int id);

        // Business Logic
        Task AssignAssetAsync(int assetId, int employeeId, string notes);
        Task<IEnumerable<Asset>> GetAvailableAssetsAsync();
    }
}