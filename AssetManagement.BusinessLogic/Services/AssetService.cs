using AssetManagement.Core.Entities;
using AssetManagement.DataAccess;
using AssetManagement.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.BusinessLogic.Services
{
    public class AssetService : IAssetService
    {
        private readonly IGenericRepository<Asset> _assetRepository;
        private readonly IGenericRepository<AssetAssignmentHistory> _historyRepository;
        private readonly ApplicationDbContext _context;

        public AssetService(IGenericRepository<Asset> assetRepository, IGenericRepository<AssetAssignmentHistory> historyRepository, ApplicationDbContext context)
        {
            _assetRepository = assetRepository;
            _historyRepository = historyRepository;
            _context = context;
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

        public async Task<IEnumerable<Asset>> SearchAssetsAsync(string? serialNumber, string? status, string? assetType)
        {
            var query = _context.Assets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                query = query.Where(a => a.SerialNumber.Contains(serialNumber));
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                query = query.Where(a => a.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(assetType))
            {
                query = query.Where(a => a.AssetType.Contains(assetType));
            }

            return await query.ToListAsync();
        }
    }
}