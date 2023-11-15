using Moq;

namespace TelegramBot.Test
{

    [TestClass]
    public class ParserTests
    {
        private const string ValidCurrencyCode = "USD";
        private const string ValidDate = "01.01.2022";
        private const string InvalidDate = "invalid date";
        private const long Id = 1;

        private Mock<IPrivatbankApi> _mockPrivatbankApi;
        private Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _mockPrivatbankApi = new Mock<IPrivatbankApi>();
            _parser = new Parser(_mockPrivatbankApi.Object);
        }

        [TestMethod]
        public async Task ParsUserEnter_DataExeption_NotValidDate()
        {
            var result = await _parser.ParsUserEnter(InvalidDate, Id);
            Assert.AreEqual(Constants.DataExeption, result);
        }

        [TestMethod]
        public async Task ParsUserEnter_DataOutOfRange_Exception()
        {
            _mockPrivatbankApi.Setup(x => x.GetApiData(ValidDate)).ThrowsAsync(new Exception());

            var result = await _parser.ParsUserEnter(ValidCurrencyCode + " " + ValidDate, Id);

            Assert.AreEqual(Constants.DataOutOfRange, result);
        }

        [TestMethod]
        public async Task ParsUserEnter_CurrencyCodExeption()
        {
            var header = new Header
            {
                ExchangeRate = new[]
                {
                new ExchangeRate { Currency = "EUR", SaleRateNB = 2.0, PurchaseRateNB = 2.0 }
                }
            };

            _mockPrivatbankApi.Setup(x => x.GetApiData(ValidDate)).ReturnsAsync(header);

            var result = await _parser.ParsUserEnter(ValidCurrencyCode + " " + ValidDate, Id);

            Assert.AreEqual(Constants.CurrencyCodExeption + Environment.NewLine + "EUR" + Environment.NewLine, result);
        }

        [TestMethod]
        public async Task ParsUserEnter_ExchangeRates_()
        {
            var header = new Header
            {
                ExchangeRate = new[]
                {
                new ExchangeRate { Currency = ValidCurrencyCode, SaleRateNB = 2.0, PurchaseRateNB = 2.0 }
                }
            };

            _mockPrivatbankApi.Setup(x => x.GetApiData(ValidDate)).ReturnsAsync(header);

            var result = await _parser.ParsUserEnter(ValidCurrencyCode + " " + ValidDate, Id);

            Assert.AreEqual($"NBU sales rate: {header.ExchangeRate[0].SaleRateNB}. NBU purchase price: {header.ExchangeRate[0].PurchaseRateNB}", result);
        }
    }

}
