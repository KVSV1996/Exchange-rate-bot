using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Test
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void Token_ShouldBeLoadedFromConfigurationFile()
        {           
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            var config = builder.Build();
            var expectedToken = config.GetSection("Token").Get<string>();

            var configuration = new Configuration();
                        
            var actualToken = configuration.Token;
            
            Assert.AreEqual(expectedToken, actualToken);
        }
    }
}
