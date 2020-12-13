using System;
using Microsoft.Extensions.DependencyInjection;
using Api.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Test
{
    public abstract class BaseTest
    {
        public BaseTest()
        {

        }
    }
    public class DbTest : IDisposable
    {
        private string databaseName = $"dbApiTest_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
        public ServiceProvider ServiceProvider { get; private set; }

        public void Dispose()
        {
            using (var context = ServiceProvider.GetService<MyContext>())
            {
                context.Database.EnsureDeleted();
            }
        }
        public DbTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MyContext>(o =>
                o.UseMySql($"Persist Security Info=True;Server=localhost;Database={databaseName};User=root;Password=joaopjk",
                new MySqlServerVersion(new System.Version(8,0,22))),
                ServiceLifetime.Transient
            );
            ServiceProvider = serviceCollection.BuildServiceProvider();
            using (var context = ServiceProvider.GetService<MyContext>())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}