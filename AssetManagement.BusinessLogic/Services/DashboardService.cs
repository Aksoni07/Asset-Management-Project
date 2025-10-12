using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AssetManagement.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDbConnection _db;

        public DashboardService(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> GetTotalAssetCountAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets");
        }

        public async Task<int> GetAssignedAssetCountAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets WHERE Status = 'Assigned'");
        }

        public async Task<int> GetAvailableAssetsAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets WHERE Status = 'Available'");
        }

        public async Task<int> GetRetiredAssetCountAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets WHERE Status = 'Retired'");
        }

        public async Task<int> GetUnderRepairAssetCountAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets WHERE Status = 'Under Repair'");
        }
         public async Task<int> GetSpareAssetCountAsync()
        {
            return await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Assets WHERE IsSpare = 1");
        }

        public async Task<Dictionary<string, int>> GetAssetCountByTypeAsync()
        {
            var queryResult = await _db.QueryAsync<AssetTypeCount>(
                "SELECT AssetType, COUNT(*) as Count FROM Assets GROUP BY AssetType"
            );
            return queryResult.ToDictionary(r => r.AssetType, r => r.Count);
        }

        private class AssetTypeCount
        {
            public string AssetType { get; set; } = string.Empty;
            public int Count { get; set; }
        }
    }
}