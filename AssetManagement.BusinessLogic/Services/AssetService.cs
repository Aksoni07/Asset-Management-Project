using AssetManagement.Core.Entities;
using AssetManagement.DataAccess.Repositories;

namespace AssetManagement.BusinessLogic.Services
{
    public class AssetService : IAssetService
    {
        private readonly IGenericRepository<Asset> _assetRepository;
        private readonly IGenericRepository<AssetAssignmentHistory> _historyRepository;
        
        public AssetService(IGenericRepository<Asset> assetRepository, IGenericRepository<AssetAssignmentHistory> historyRepository)
        {
            _assetRepository = assetRepository;
            _historyRepository = historyRepository;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await _assetRepository.GetAllAsync();
        }

        public async Task<Asset> GetAssetByIdAsync(int id)
        {
            return await _assetRepository.GetByIdAsync(id);
        }

        public async Task AddAssetAsync(Asset asset)
        {
            await _assetRepository.AddAsync(asset);
            await _assetRepository.SaveChangesAsync();
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            _assetRepository.Update(asset);
            await _assetRepository.SaveChangesAsync();
        }

        public async Task DeleteAssetAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset != null)
            {
                _assetRepository.Delete(asset);
                await _assetRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Asset>> GetAvailableAssetsAsync()
        {
            var allAssets = await _assetRepository.GetAllAsync();
            return allAssets.Where(a => a.Status == "Available");
        }

        public async Task AssignAssetAsync(int assetId, int employeeId, string notes)
        {
            var asset = await _assetRepository.GetByIdAsync(assetId);
            if (asset == null || asset.Status != "Available")
            {
                throw new InvalidOperationException("Asset is not available for assignment.");
            }

            asset.Status = "Assigned";
            _assetRepository.Update(asset);

            var history = new AssetAssignmentHistory
            {
                AssetId = assetId,
                EmployeeId = employeeId,
                AssignedDate = DateTime.UtcNow,
                Notes = notes
            };
            await _historyRepository.AddAsync(history);

            await _assetRepository.SaveChangesAsync();
        }
    }
}