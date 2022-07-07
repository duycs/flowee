using AppShareServices.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace SpecificationAPI.SeedData
{
    public class SpecificationContextSeed : SeedDataBase
    {
        public SpecificationContextSeed(DbContext dbContext, string contentRootPath, string seedDataFolder) : base(dbContext, contentRootPath, seedDataFolder)
        {
        }

        public override Task SeedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
