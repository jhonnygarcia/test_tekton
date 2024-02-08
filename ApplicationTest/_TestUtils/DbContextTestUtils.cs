using Microsoft.EntityFrameworkCore;

namespace ApplicationTest._TestUtils
{
    public static class DbContextTestUtils
    {
        public static T GetDbContextMock<T>()
            where T : DbContext
        {
            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return (T)Activator.CreateInstance(typeof(T), options);
        }
    }
}
