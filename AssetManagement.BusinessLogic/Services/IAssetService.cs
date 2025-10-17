using AssetManagement.Core;
using AssetManagement.Core.Entities; // Interface must know the shape of entity on which it is going to work

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
        
        // Business Logic --> to Assign Asset , to Get Available Assets
        Task AssignAssetAsync(int assetId, int employeeId, string notes); // Check Available Assets, Update  Status, update 1 entry in: AssetAssignmentHistory
        Task<IEnumerable<Asset>> GetAvailableAssetsAsync();              // for Asset Assignemtn to Emp
        Task ReturnAssetAsync(int assetId);                             // find active (Assigned) Assets, set its Return Date , make status: Available

        // Search Method --.> Pagenation , Filtering , Sort
        Task<PagedResult<Asset>> SearchAssetsAsync(
            string? serialNumber, 
            string? status, 
            string? assetType, 
            int? employeeId, 
            int pageNumber, 
            int pageSize, 
            string sortBy, 
            bool isAscending);  // return PagedResult object --> 1. List of assets for current Page  + 2. total number of assets (All Pages)
            
        // Report Methods ---> History , Warranty
        Task<IEnumerable<AssetAssignmentHistory>> GetAssignmentHistoryAsync(int assetId);
        Task<IEnumerable<Asset>> GetAssetsNearingWarrantyExpiryAsync(int days);
        
    }
}