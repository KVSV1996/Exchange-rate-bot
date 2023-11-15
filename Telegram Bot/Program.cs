using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Telegram.Bot;

namespace TelegramBot
{
    public class Program
    {        
        public static void Main(string[] args)
        {        
            Configuration configuration = new ();
            var serviceProvider = new ServiceCollection()
            .AddSingleton(x => RestService.For<IPrivatbankApi>("https://api.privatbank.ua"))  
            .AddSingleton<ITelegramBotClient>(t => new TelegramBotClient(configuration.Token))            
            .AddSingleton<ICommunication, ConsoleCommunication>()
            .AddSingleton<ProgramManager>()
            .BuildServiceProvider();

            var manager = serviceProvider.GetRequiredService<ProgramManager>();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            manager.InitialiseBot();            

            Log.CloseAndFlush();
        }
    }
}
