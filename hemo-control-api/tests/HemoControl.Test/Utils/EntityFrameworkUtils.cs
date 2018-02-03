using Microsoft.EntityFrameworkCore;

namespace HemoControl.Test.Utils
{
    public static class EntityFrameworkUtils
    {
        public static DbContextOptions<T> CreateInMemoryDbOptions<T>(string dbName)
             where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }
    }
}