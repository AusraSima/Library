using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryConsoleApp
{
	public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
	{
		public LibraryDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			string connectionString = configuration.GetConnectionString("DefaultConnection");
			var builder = new DbContextOptionsBuilder<LibraryDbContext>();
			builder.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 32)));
			return new LibraryDbContext(builder.Options);
		}
	}
}
