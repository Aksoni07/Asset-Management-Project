using AssetManagement.Core.Entities; // import the shape of our Entities to Map them into Db schema
using Microsoft.EntityFrameworkCore; //import lib to support classes: DbContext, DbSet, and ModelBuilder

namespace AssetManagement.DataAccess
{
    public class ApplicationDbContext : DbContext // Inheriting the build in functionalities of DbContext class in our own class
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Contructor: Read Connection string from "appsetting.json" when system starts : packages it into an options object --> pass to DbContext.
        // Ensure how to connect with DB

        public DbSet<Employee> Employees { get; set; }   // Mapping of Core Entities with Db Tables:  TablesDbSet<Employee> maps to the Employees table.
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAssignmentHistory> AssetAssignmentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // inBuild Method: OnModelCreating Overrided: define database rules
        {
            base.OnModelCreating(modelBuilder); // default Implementation : Set Relationship , check valdiation (Required), Set PK ,config Column name + Datatype

            // Configure unique constraint for SerialNumber on Asset 
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.SerialNumber)
                .IsUnique();
        }
    }
}

/*
Connect application to database 
Translate :  C# objects:Employee ---> database table rows.
Manage Conversation/Changes With DB
Save Changes into DB
*/

