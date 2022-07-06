using AppShareDomain.Models;
using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using CustomerInfrastructure;
using System.Text.RegularExpressions;

namespace CustomerAPI.SeedData
{
    public class CustomerContextSeed : SeedDataBase
    {
        private CustomerContext _context;
        private ILogger<CustomerContextSeed> _logger;
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;

        public CustomerContextSeed(CustomerContext context, ILogger<CustomerContextSeed> logger,
            IWebHostEnvironment hostEnvironment, IConfiguration configuration, string contentRootPath, string seedDataFolder)
            : base(context, contentRootPath, seedDataFolder)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public override async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(CustomerContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                // Master data
                if (!_context.CardTypes.Any())
                {
                    var cardTypes = GetCardTypesFromFile();
                    if (cardTypes.Any())
                    {
                        await _context.CardTypes.AddRangeAsync(cardTypes);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Currencies.Any())
                {
                    var currencies = GetCurrenciesFromFile();
                    if (currencies.Any())
                    {
                        await _context.Currencies.AddRangeAsync(currencies);
                        _context.SaveChanges();
                    }
                }

                if (!_context.PriorityLevels.Any())
                {
                    var priorityLevels = GetPriorityLevelsFromFile();
                    if (priorityLevels.Any())
                    {
                        await _context.PriorityLevels.AddRangeAsync(priorityLevels);
                        _context.SaveChanges();
                    }
                }

                if (!_context.PaymentMethods.Any())
                {
                    var paymentMethods = GetPaymentMethodsFromFile();
                    if (paymentMethods.Any())
                    {
                        await _context.PaymentMethods.AddRangeAsync(paymentMethods);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Customers.Any())
                {
                    var customers = GetCustomersFromFile();
                    if (customers.Any())
                    {
                        await _context.Customers.AddRangeAsync(customers);
                        _context.SaveChanges();
                    }
                }

            });
        }


        // TODO
        private IEnumerable<T> GetEEnumrationNameFromFile<T>(string fileName, Func<string, int, T> createEntityFunction) where T : Enumeration
        {
            string csvFile = GetPathToFile(fileName);
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<T>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => createEntityFunction(x, id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private IEnumerable<Customer> GetCustomersFromFile()
        {
            string csvFile = GetPathToFile("Customers.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Customer>();
            }

            string[] requiredHeaders = { "FirstName", "LastName", "Email", "Phone", "Description", "Currency", "PriorityLevel", "PaymentMethodIds" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateCustomer(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private IEnumerable<PaymentMethod> GetPaymentMethodsFromFile()
        {
            string csvFile = GetPathToFile("PaymentMethods.csv");

            if (!File.Exists(csvFile))
            {
                return new List<PaymentMethod>();
            }

            string[] requiredHeaders = { "Alias", "CardNumber", "SecurityNumber", "CardHolderName", "CardType" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreatePaymentMethod(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private Customer CreateCustomer(string[] column, string[] headers)
        {
            var paymentMethodIdsValue = column[Array.IndexOf(headers, "PaymentMethodIds".ToLower())].Trim('"').Trim();
            var paymentMethodIdsAsListString = paymentMethodIdsValue.Split(",");

            var paymentMethodIds = new List<int>();
            if (paymentMethodIdsAsListString.Any())
            {
                paymentMethodIds = paymentMethodIdsAsListString.Select(Int32.Parse).ToList();
            }
            else
            {
                paymentMethodIds.Add(Int32.Parse(paymentMethodIdsValue));
            }

            var paymentMethods = _context.PaymentMethods.Where(w => paymentMethodIds.Contains(w.Id)).ToList();
            if (!paymentMethods.Any())
            {
                throw new Exception("paymentMethodIds is null or empty");
            }

            var customer = new Customer()
            {
                FirstName = column[Array.IndexOf(headers, "FirstName".ToLower())].Trim('"').Trim(),
                LastName = column[Array.IndexOf(headers, "LastName".ToLower())].Trim('"').Trim(),
                Email = column[Array.IndexOf(headers, "Email".ToLower())].Trim('"').Trim(),
                Phone = column[Array.IndexOf(headers, "Phone".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                Currency = Enumeration.FromDisplayName<Currency>(column[Array.IndexOf(headers, "Currency".ToLower())].Trim('"').Trim()),
                PriorityLevel = Enumeration.FromDisplayName<PriorityLevel>(column[Array.IndexOf(headers, "PriorityLevel".ToLower())].Trim('"').Trim()),
                PaymentMethods = paymentMethods,
                DateCreated = DateTime.UtcNow,
            };

            return customer;
        }

        private PaymentMethod CreatePaymentMethod(string[] column, string[] headers)
        {
            var paymentMethod = new PaymentMethod()
            {
                Alias = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                CardNumber = column[Array.IndexOf(headers, "CardNumber".ToLower())].Trim('"').Trim(),
                SecurityNumber = column[Array.IndexOf(headers, "SecurityNumber".ToLower())].Trim('"').Trim(),
                CardHolderName = column[Array.IndexOf(headers, "CardHolderName".ToLower())].Trim('"').Trim(),
                CardType = Enumeration.FromDisplayName<CardType>(column[Array.IndexOf(headers, "CardType".ToLower())].Trim('"').Trim()),
                Expiration = DateTime.UtcNow.AddYears(5),
                DateCreated = DateTime.UtcNow,
            };

            return paymentMethod;
        }


        private IEnumerable<CardType> GetCardTypesFromFile()
        {
            string csvFile = GetPathToFile("CardTypes.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<CardType>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateCardType(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
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
                                        .Where(x => x != null);
        }

        private IEnumerable<PriorityLevel> GetPriorityLevelsFromFile()
        {
            string csvFile = GetPathToFile("PriorityLevels.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<PriorityLevel>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreatePriorityLevels(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }


        private CardType CreateCardType(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("CardType is null or empty");
            }

            return new CardType(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private Currency CreateCurrency(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("Currency is null or empty");
            }

            return new Currency(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private PriorityLevel CreatePriorityLevels(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("PriorityLevel is null or empty");
            }

            return new PriorityLevel(id++, value.Trim('"').Trim().ToLowerInvariant());
        }
    }
}
