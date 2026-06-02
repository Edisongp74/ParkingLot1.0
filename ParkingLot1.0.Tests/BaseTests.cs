using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Tests
{
    public class BaseTests
    {
        protected static ApplicationDbContext BuildContext(string? dbName = null)
        {
            dbName = dbName ?? Guid.NewGuid().ToString();

            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            return context;
        }

        protected static async Task SaveChangesAsync(ApplicationDbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
