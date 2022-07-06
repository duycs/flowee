using AppShareServices.DataAccess;
using Microsoft.EntityFrameworkCore;
using ProductInfrastructure.DataAccess;

namespace ProductAPI.SeedData
{
    public class ProductContextSeed : SeedDataBase
    {
        private readonly ProductContext _dbContext;
        private string _contentRootPath;
        private string _seedDataFolder;

        public ProductContextSeed(ProductContext dbContext, string contentRootPath, string seedDataFolder) : base(dbContext, contentRootPath, seedDataFolder)
        {
            _dbContext = dbContext;
            _contentRootPath = contentRootPath;
            _seedDataFolder = seedDataFolder;
        }

        public override Task SeedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
