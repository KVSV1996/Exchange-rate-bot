using Serilog;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public class ProgramManager
    {
        private Parser _parser;        
        private readonly IPrivatbankApi _privatbankApi;
        private readonly ICommunication _communication;
        private readonly ITelegramBotClient _botClient;

        public ProgramManager(IPrivatbankApi privatbankApi, ICommunication communication, ITelegramBotClient botClient)
        {
            this._privatbankApi = privatbankApi ?? throw new ArgumentNullException(nameof(privatbankApi));             
            this._communication = communication ?? throw new ArgumentNullException(nameof(communication));
            this._botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        }       

        public void InitialiseBot()
        {                     
            _botClient.StartReceiving(UpdateAsync, Exeption);
            _communication.ReadLine();
        }     


        private async Task UpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {           
            var massage = update.Message;

            if (massage == null)
            {
                return;
            }

            if (massage.Text != null)
            {
                if (massage.Text == "/start")
                {
                    Log.Information($"ID[{massage.Chat.Id}]: /start");
                    await botClient.SendTextMessageAsync(massage.Chat.Id, Constants.Head);
                    return;
                }

                await UserAnswerAsync(massage, botClient);
            }
            else
            {
                Log.Error($"ID[{massage.Chat.Id}]: TypeException. User enter: {massage.Text}");
                await botClient.SendTextMessageAsync(massage.Chat.Id, Constants.TypeException);
            }
        }

        private async Task UserAnswerAsync(Message massage, ITelegramBotClient botClient)
        {
            _parser = new Parser(_privatbankApi);
            string paternForText = @"\w{3}\s\d{2}.\d{2}.\d{4}";

            if (!Regex.IsMatch(massage.Text, paternForText))
            {
                Log.Error($"ID[{massage.Chat.Id}]: FormatExeption. User enter: {massage.Text}");
                await botClient.SendTextMessageAsync(massage.Chat.Id, Constants.FormatExeption);
                return;
            }
            else
            {
                string data = await _parser.ParsUserEnter(massage.Text, massage.Chat.Id);
                await botClient.SendTextMessageAsync(massage.Chat.Id, data);
                return;
            }
        }

        private Task Exeption(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
