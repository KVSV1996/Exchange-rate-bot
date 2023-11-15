using Serilog;
using System.Globalization;

namespace TelegramBot
{
    public class Parser
    {
        private const int LenghtOfCode = 3;
        private const int FirstIndexOfDate = 4;

        private Header _header;
        private readonly IPrivatbankApi _privatbankApi;
        public Parser(IPrivatbankApi privatbankApi)
        {
           this._privatbankApi = privatbankApi ?? throw new ArgumentNullException(nameof(privatbankApi));
        }

        public async Task<string> ParsUserEnter(string text, long id)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            string currencyCode = text.Substring(0, LenghtOfCode);
            string date = text.Substring(FirstIndexOfDate);
            string currentCode = Environment.NewLine;

            if (TextIsDate(date))
            {
                try
                {
                    _header = await _privatbankApi.GetApiData(date);
                    
                    foreach (var d in _header.ExchangeRate)
                    {
                        if (d.Currency == currencyCode)
                        {
                            string answer = $"NBU sales rate: {d.SaleRateNB}. NBU purchase price: {d.PurchaseRateNB}";
                            Log.Information($"ID[{id}]: User enter: {text}");
                            Log.Information($"ID[{id}]: " + answer);
                            return answer;
                        }
                    }

                    Log.Error($"ID[{id}]: CurrencyCodExeption. User enter: {currencyCode}");                    

                    foreach (var d in _header.ExchangeRate)
                    {
                        currentCode += d.Currency + Environment.NewLine;
                    }

                    return Constants.CurrencyCodExeption + currentCode;
                }
                catch
                {
                    Log.Error($"ID[{id}]: DataOutOfRange User enter: {date}");
                    return Constants.DataOutOfRange;
                }
            }
            return Constants.DataExeption;
        }

        private bool TextIsDate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            var trimText = text.Trim();
            var dateFormat = "dd.MM.yyyy";
            DateTime scheduleDate;
            return DateTime.TryParseExact(trimText, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate);
        }
    }
}
