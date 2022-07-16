using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductDomain.AgreegateModels.ProductAgreegate;
using ProductInfrastructure.DataAccess;
using System.Text.RegularExpressions;

namespace ProductAPI.SeedData
{
    public class ProductContextSeed : SeedDataBase
    {
        private readonly ProductContext _dbContext;
        private ILogger<ProductContextSeed> _logger;

        public ProductContextSeed(ProductContext dbContext, string contentRootPath, string seedDataFolder, ILogger<ProductContextSeed> logger) : base(dbContext, contentRootPath, seedDataFolder)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(ProductContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!_dbContext.Categories.Any())
                {
                    var addons = GetCategoryFromFile();
                    if (addons.Any())
                    {
                        await _dbContext.Categories.AddRangeAsync(addons);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var currencies = GetProductFromFile();
                    if (currencies.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(currencies);
                        _dbContext.SaveChanges();
                    }
                }
            });
        }

        private IEnumerable<Category> GetCategoryFromFile()
        {
            string csvFile = GetPathToFile("Categories.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Category>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateCategory(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private IEnumerable<Product> GetProductFromFile()
        {
            string csvFile = GetPathToFile("Products.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Product>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description", "CatalogId", "CustomerId", "CategoryIds" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateProduct(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private Category CreateCategory(string[] column, string[] headers)
        {
            var category = new Category()
            {
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return category;
        }

        private Product CreateProduct(string[] column, string[] headers)
        {
            var product = new Product()
            {
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                CatalogId = int.Parse(column[Array.IndexOf(headers, "CatalogId".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return product;
        }
    }
}
