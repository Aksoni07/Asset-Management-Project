using AssetManagement.Core;
using AssetManagement.Core.Entities;
using AssetManagement.DataAccess;
using AssetManagement.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore; // AsQueryable(), Include(), ToListAsync(), CountAsync(), FindAsync()
using System.Linq.Expressions; // Expression<Func<>> :sorting: sortBy, OrderBy : Switch statement

namespace AssetManagement.BusinessLogic.Services
{
    public class AssetService : IAssetService
    {
        private readonly IGenericRepository<Asset> _assetRepository;
        private readonly IGenericRepository<AssetAssignmentHistory> _historyRepository;
        private readonly ApplicationDbContext _context; // for complex quiries which Igeneric cant handle : Search + Sort + Filter

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
            var history = new AssetAssignmentHistory { AssetId = assetId, EmployeeId = employeeId, AssignedDate = DateTime.UtcNow, Notes = notes };
            await _historyRepository.AddAsync(history);
            await _assetRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<Asset>> SearchAssetsAsync(string? serialNumber, string? status, string? assetType, int? employeeId, int pageNumber, int pageSize, string sortBy, bool isAscending) // Total Cnt of Items , Items for Curr Page
        {
            var query = _context.Assets.AsQueryable(); // help to build query step by step

            if (!string.IsNullOrWhiteSpace(serialNumber)) { query = query.Where(a => a.SerialNumber.Contains(serialNumber)); }

            if (!string.IsNullOrWhiteSpace(status) && status != "All") { query = query.Where(a => a.Status == status); }

            if (!string.IsNullOrWhiteSpace(assetType)) { query = query.Where(a => a.AssetType.Contains(assetType)); }

            if (employeeId.HasValue && employeeId > 0)
            {
                var assignedAssetIds = await _context.AssetAssignmentHistories
                    .Where(h => h.EmployeeId == employeeId.Value && h.ReturnedDate == null)
                    .Select(h => h.AssetId)
                    .ToListAsync();
                query = query.Where(a => assignedAssetIds.Contains(a.AssetId));
            }

            Expression<Func<Asset, object>> keySelector = sortBy?.ToLower() switch
            {
                "id" => a => a.AssetId,
                "type" => a => a.AssetType,
                "serial" => a => a.SerialNumber,
                "status" => a => a.Status,
                _ => a => a.AssetName,
            };// Dynamic sorting : Expression<Func<Asset, object>> : build dynamic quiries, so Entity Framework can Understand it and translate to SQL quireis
            query = isAscending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Asset> { Items = items, TotalCount = totalCount };
        }

        public async Task<IEnumerable<AssetAssignmentHistory>> GetAssignmentHistoryAsync(int assetId)
        {
            return await _context.AssetAssignmentHistories 
            //search in tbl Asset assignment histories
                .Include(h => h.Employee) // Join: Employee + HistoryTable --> load data of Employees
                .Where(h => h.AssetId == assetId)
                .OrderByDescending(h => h.AssignedDate) // quiry Build
                .ToListAsync(); // quiry sent to db --> return list of Obj
        }

        public async Task<IEnumerable<Asset>> GetAssetsNearingWarrantyExpiryAsync(int days)
        {
            var expiryThreshold = DateTime.UtcNow.AddDays(days);
            return await _context.Assets // Search of on Asset Table
                .Where(a => a.WarrantyExpiryDate <= expiryThreshold && a.WarrantyExpiryDate >= DateTime.UtcNow)
                .OrderBy(a => a.WarrantyExpiryDate)
                .ToListAsync();
        }

        public async Task ReturnAssetAsync(int assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null || asset.Status != "Assigned")
            {
                return;
            }

            var currentAssignment = await _context.AssetAssignmentHistories
                .FirstOrDefaultAsync(h => h.AssetId == assetId && h.ReturnedDate == null);

            if (currentAssignment != null)
            {
                currentAssignment.ReturnedDate = DateTime.UtcNow;
                _context.AssetAssignmentHistories.Update(currentAssignment);
            }

            asset.Status = "Available";
            _context.Assets.Update(asset);

            await _context.SaveChangesAsync(); // ATOMIC TRANSACTION --> UPDATE HISTORY + CHANGE ASSET STATUR
        }
    }
}