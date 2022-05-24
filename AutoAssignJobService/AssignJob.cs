
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text.Json;

namespace AutoAssignJobService
{
    public class AssignJob
    {
        /* Global instance of the scopes required by this quickstart.
         If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static void Run()
        {
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define request parameters.
                const string SPREADSHEET_ID = "1eR-eOgG09RG8k8JDZWoPDoU0ovnsBTMvJ7DC5d3kcwE";
                const string SHEET_NAME = "Items";
                var range = $"{SHEET_NAME}!A:D";

                var googleSheetValues = service.Spreadsheets.Values;
                var getRequest = googleSheetValues.Get(SPREADSHEET_ID, range);

                ValueRange getResponse = getRequest.Execute();
                IList<IList<Object>> values = getResponse.Values;
                if (values == null || values.Count == 0)
                {
                    Console.WriteLine("No data found.");
                    return;
                }
                Console.WriteLine(JsonSerializer.Serialize(values));


                var values2 = new List<List<object>>()
                {
                    new List<object>() { "1", "2" }
                };
                List<ValueRange> data = new List<ValueRange>();
                data.Add(new ValueRange()
                {
                    Range = range,
                    Values = (IList<IList<object>>)values2,
                });

                BatchUpdateValuesRequest body = new BatchUpdateValuesRequest()
                {
                    ValueInputOption = "10",
                    Data = data
                };

                BatchUpdateValuesResponse result =
                        googleSheetValues.BatchUpdate(body, SPREADSHEET_ID).Execute();

                Console.WriteLine(result);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}