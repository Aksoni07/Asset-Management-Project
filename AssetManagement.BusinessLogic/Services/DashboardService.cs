// Execute: Simple , Very Fast - read only quiries, return numbres on DashBoard

using Dapper; // high-performance queries: No Overhead--> can run direct SQL quiries: QueryAsync, ExecuteScalarAsync
using Microsoft.Extensions.Configuration; // access : appsettings.json -> Db Connection string
using System.Data;// provide db connection interface: IDbConnection
using Microsoft.Data.SqlClient;// provide SqlConnection class: for connecting to a Microsoft SQL Server DB

namespace AssetManagement.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDbConnection _db; // hold actual connection to database : SQLConnection

        public DashboardService(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")); // read db Connection String from 'appsettings.json'
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
            );// QueryAsync: return multiple rows and columns
            return queryResult.ToDictionary(r => r.AssetType, r => r.Count); // queryResult holds list of AssetTypeCount (List to Dict)
        }

        private class AssetTypeCount
        {
            public string AssetType { get; set; } = string.Empty;
            public int Count { get; set; }
        }// temp class to hold the list of result given by dapper method
    }
}