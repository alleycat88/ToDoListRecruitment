using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ToDoListRecruitment.Utility;

namespace ToDoListRecruitment
{
    public class Program
    {
        public static dynamic settings = new SettingsController().LoadSettings();
        public static HttpClient httpClient = new HttpClient();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseUrls("http://0.0.0.0:"+(string)settings.port);
                });
    }
}
