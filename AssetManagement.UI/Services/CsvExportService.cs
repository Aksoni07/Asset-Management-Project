using AssetManagement.Core.Entities;
using System.Text;

namespace AssetManagement.UI.Services
{
    public class CsvExportService // take a list of C# objects --> do Formatting in csv
    {
        public byte[] ExportAssetsToCsv(IEnumerable<Asset> assets)
        {
            var builder = new StringBuilder();//Building a large text file
            // Add the header row
            builder.AppendLine("AssetName,AssetType,MakeModel,SerialNumber,PurchaseDate,WarrantyExpiryDate,Condition,Status");

            // Add the data rows
            foreach (var asset in assets)
            {
                builder.AppendLine($"{Escape(asset.AssetName)},{Escape(asset.AssetType)},{Escape(asset.MakeModel)},{Escape(asset.SerialNumber)},{asset.PurchaseDate:yyyy-MM-dd},{asset.WarrantyExpiryDate:yyyy-MM-dd},{Escape(asset.Condition)},{Escape(asset.Status)}");
            }

            return Encoding.UTF8.GetBytes(builder.ToString());//text into the raw bytes
        }

        // Helper to handle commas or quotes in the data
        private string Escape(string value)
        {
            if (value.Contains(',') || value.Contains('"'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
}