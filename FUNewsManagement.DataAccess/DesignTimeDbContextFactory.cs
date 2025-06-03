using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FUNewsManagement.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FUNewsManagementDbContext>
    {
        public FUNewsManagementDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FUNewsManagementDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new FUNewsManagementDbContext(builder.Options);
        }
    }
} 