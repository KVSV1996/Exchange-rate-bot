using Moq;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Test
{
    [TestClass]
    public class ProgramManagerTests
    {
        private Mock<IPrivatbankApi> _privatbankApiMock;
        private Mock<ICommunication> _communicationMock;
        private Mock<ITelegramBotClient> _botClientMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _privatbankApiMock = new Mock<IPrivatbankApi>();
            _communicationMock = new Mock<ICommunication>();
            _botClientMock = new Mock<ITelegramBotClient>();
        }

        [TestMethod]
        public void InitialiseBot_StartReceiving_Called()
        {
            this._communicationMock.SetupSequence(x => x.ReadLine())
                .Returns(" ");
            
            var programManager = new ProgramManager(_privatbankApiMock.Object, _communicationMock.Object, _botClientMock.Object);
           
            programManager.InitialiseBot();
            
            Assert.IsTrue(true);
        }
    }
}