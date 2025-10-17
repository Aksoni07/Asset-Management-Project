namespace AssetManagement.BusinessLogic.Services
{
    public interface IDashboardService
    {
        Task<int> GetTotalAssetCountAsync();
        Task<int> GetAssignedAssetCountAsync();
        Task<int> GetAvailableAssetsAsync();
        Task<int> GetUnderRepairAssetCountAsync();
        Task<int> GetRetiredAssetCountAsync();
        Task<int> GetSpareAssetCountAsync();
        Task<Dictionary<string, int>> GetAssetCountByTypeAsync(); // get the count of assets grouped by Type return (Type : cnt)
    }
} // read-only service : Dapper --> uses optimised Quireies SQL : Count and Grouped By --> fast