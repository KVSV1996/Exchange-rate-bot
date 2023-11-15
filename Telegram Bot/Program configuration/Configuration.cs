
using Microsoft.Extensions.Configuration;

namespace TelegramBot
{
    public class Configuration
    {
        public Configuration()
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

           Token = config.GetSection("Token").Get<string>();
        }

        public string Token { get; set; }
    }
}
