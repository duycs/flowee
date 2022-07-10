using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;

namespace AppShareServices.Extensions
{
    public static class CsvExtensions
    {
        public static string[] GetHeaders(this string csvfile, string[] requiredHeaders, string[]? optionalHeaders = null)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader.ToLower()))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        public static string GetPathToFile(string csvFile, string contentRootPath, string seedDataFolder)
        {
            return Path.Combine(contentRootPath, seedDataFolder, csvFile);
        }

        public static string GetColumnValue(string[] columns, string[] headers, string columnName)
        {
            return columns[Array.IndexOf(headers, columnName.ToLower())].Trim('"').Trim();
        }

        public static string GetColumnValueIgnoreCase(string[] columns, string[] headers, string columnName)
        {
            return columns[Array.IndexOf(headers, columnName.ToLower())].Trim('"').Trim().ToLower();
        }
    }
}
