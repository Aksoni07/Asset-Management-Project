using AssetManagement.Core.Entities;
using AssetManagement.DataAccess;

namespace AssetManagement.UI.Data
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app) // app: holds the "keys" to all the services we configured in Program.cs
        {
            using (var serviceScope = app.ApplicationServices.CreateScope()) //  isolated workspace. : lives forever not like ApplicationDbContext 
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context == null) return;

                // Ensure the database is created: before the seeder tries to add data to it.
                context.Database.EnsureCreated();

                // Seed Employees if the table is empty
                if (!context.Employees.Any())
                {
                    context.Employees.AddRange(
                        new Employee { FullName = "Admin User", Department = "IT", Email = "admin@test.com", Designation = "Administrator", PhoneNumber = "111-222-3333" },
                        new Employee { FullName = "John Doe", Department = "Sales", Email = "john@test.com", Designation = "Sales Rep", PhoneNumber = "444-555-6666" }
                    );
                    context.SaveChanges();
                }

                // Seed Assets if the table is empty
                if (!context.Assets.Any())
                {
                    context.Assets.AddRange(
                         new Asset { AssetName = "Dev Laptop", AssetType = "Laptop", MakeModel = "Dell XPS 15", SerialNumber = "SN12345", Status = "Available", Condition = "New", PurchaseDate = DateTime.Now.AddYears(-1), WarrantyExpiryDate = DateTime.Now.AddYears(2), Specifications = "16GB RAM, 512GB SSD" },
                         new Asset { AssetName = "Marketing Monitor", AssetType = "Monitor", MakeModel = "LG UltraWide", SerialNumber = "SN67890", Status = "Available", Condition = "Good", PurchaseDate = DateTime.Now.AddYears(-2), WarrantyExpiryDate = DateTime.Now.AddYears(1), Specifications = "34-inch QHD" }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}