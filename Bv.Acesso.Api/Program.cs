using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bv.Acesso.Api
{
    public class Program
    {
        public static void Main(string[] args)=> GetHostBuilder(args).Build().Run();

        public static IHostBuilder GetHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureWebHostDefaults(webBuilder =>
        {            
            webBuilder.UseUrls("https://*:32772").UseStartup<Startup>();
        });
    }
}
