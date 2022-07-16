using AppShareDomain.Models;
using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using CatalogInfrastructure.DataAccess;
using System.Text.RegularExpressions;

namespace CatalogAPI.SeedData
{
    public class CatalogContextSeed : SeedDataBase
	{
		private CatalogContext _dbContext;
		private ILogger<CatalogContextSeed> _logger;
		private IConfiguration _configuration;
		private IWebHostEnvironment _hostEnvironment;

		public CatalogContextSeed(CatalogContext dbContext, ILogger<CatalogContextSeed> logger,
			IWebHostEnvironment hostEnvironment, IConfiguration configuration, string contentRootPath, string seedDataFolder)
			: base(dbContext, contentRootPath, seedDataFolder)
		{
			_dbContext = dbContext;
			_logger = logger;
			_configuration = configuration;
			_hostEnvironment = hostEnvironment;
		}
		public override async Task SeedAsync()
		{
			var policy = CreatePolicy(_logger, nameof(CatalogContextSeed));

			await policy.ExecuteAsync(async () =>
			{
				if (!_dbContext.Currencies.Any())
				{
					var currencies = GetCurrenciesFromFile();
					if (currencies.Any())
					{
						await _dbContext.Currencies.AddRangeAsync(currencies);
						_dbContext.SaveChanges();
					}
				}

				if (!_dbContext.Addons.Any())
				{
					var addons = GetAddonsFromFile();
					if (addons.Any())
					{
						await _dbContext.Addons.AddRangeAsync(addons);
						_dbContext.SaveChanges();
					}
				}

				if (!_dbContext.Catalogs.Any())
				{
					var catalogs = GetCatalogsFromFile();
					if (catalogs.Any())
					{
						await _dbContext.Catalogs.AddRangeAsync(catalogs);
						_dbContext.SaveChanges();
					}
				}
			});
		}

		private IEnumerable<Catalog> GetCatalogsFromFile()
		{
			string csvFile = GetPathToFile("Catalogs.csv");

			if (!File.Exists(csvFile))
			{
				return new List<Catalog>();
			}

			string[] requiredHeaders = { "Code", "Name", "Description", "PriceStandar", "Currency", "QuantityAvailable", "SpecificationId" };
			string[] headers = csvFile.GetHeaders(requiredHeaders);

			return File.ReadAllLines(csvFile)
										.Skip(1) // skip header row
										.Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
										.SelectTry(column => CreateCatalog(column, headers))
										.OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
										.Where(x => x is not null);
		}

		private IEnumerable<Addon> GetAddonsFromFile()
		{
			string csvFile = GetPathToFile("Addons.csv");

			if (!File.Exists(csvFile))
			{
				return new List<Addon>();
			}

			string[] requiredHeaders = { "Code", "Name", "Description", "Price", "Currency" };
			string[] headers = csvFile.GetHeaders(requiredHeaders);

			return File.ReadAllLines(csvFile)
										.Skip(1) // skip header row
										.Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
										.SelectTry(column => CreateAddon(column, headers))
										.OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
										.Where(x => x is not null);
		}

		private IEnumerable<Currency> GetCurrenciesFromFile()
		{
			string csvFile = GetPathToFile("Currencies.csv");
			if (!File.Exists(csvFile))
			{
				return Enumeration.GetAll<Currency>();
			}

			int id = 1;
			return File.ReadAllLines(csvFile)
										.SelectTry(x => CreateCurrency(x, ref id))
										.OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
										.Where(x => x is not null);
		}
		private Catalog CreateCatalog(string[] column, string[] headers)
		{
			var catalog = new Catalog()
			{
				Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
				Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
				Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
				QuantityAvailable = int.Parse(column[Array.IndexOf(headers, "QuantityAvailable".ToLower())].Trim('"').Trim()),
				Currency = Enumeration.FromDisplayName<Currency>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "Currency")),
				SpecificationId = int.Parse(column[Array.IndexOf(headers, "SpecificationId".ToLower())].Trim('"').Trim()),
				PriceStandar = decimal.Parse(column[Array.IndexOf(headers, "PriceStandar".ToLower())].Trim('"').Trim()),
				DateCreated = DateTime.UtcNow,
			};

			return catalog;
		}

		private Addon CreateAddon(string[] column, string[] headers)
		{
			var addon = new Addon()
			{
				Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
				Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
				Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
				SpecificationId = int.Parse(column[Array.IndexOf(headers, "SpecificationId".ToLower())].Trim('"').Trim()),
				Price = decimal.Parse(column[Array.IndexOf(headers, "Price".ToLower())].Trim('"').Trim()),
				Currency = Enumeration.FromDisplayName<Currency>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "Currency")),
				DateCreated = DateTime.UtcNow,
			};

			return addon;
		}
		private Currency CreateCurrency(string value, ref int id)
		{
			if (String.IsNullOrEmpty(value))
			{
				throw new Exception("Currency is null or empty");
			}

			return new Currency(id++, value.Trim('"').Trim().ToLowerInvariant());
		}

	}
}
