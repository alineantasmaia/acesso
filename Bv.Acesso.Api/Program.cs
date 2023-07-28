namespace Bv.Acesso.Api
{
    public class Program
    {
        public static void Main(string[] args)=> GetHostBuilder(args).Build().Run();

        public static IHostBuilder GetHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);    
    }
}
