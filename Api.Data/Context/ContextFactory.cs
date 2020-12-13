using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost;Port=3306;Database=dbApi;Uid=root;Pwd=joaopjk";
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new System.Version(8,0,22)));
            return new MyContext(optionsBuilder.Options);
        }
    }
}