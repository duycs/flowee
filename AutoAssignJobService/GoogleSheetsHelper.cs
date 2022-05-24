using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoAssignJobService
{
    public class GoogleSheetsHelper
    {
        public SheetsService Service { get; set; }
        const string APPLICATION_NAME = "GSheetAPI";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public GoogleSheetsHelper()
        {
            InitializeService();
        }
        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }
        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            return credential;
        }

        public void ReadSheet()
        {
            const string SPREADSHEET_ID = "1eR-eOgG09RG8k8JDZWoPDoU0ovnsBTMvJ7DC5d3kcwE";
            const string SHEET_NAME = "Items";

            var _googleSheetValues = new GoogleSheetsHelper().Service.Spreadsheets.Values;

            var range = $"{SHEET_NAME}!A:D";
            var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
            var response = request.Execute();
            var values = response.Values;

            Console.WriteLine(JsonSerializer.Serialize(values));
        }
    }

}
