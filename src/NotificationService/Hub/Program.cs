using Hub.Config;
using Hub.Hubs;
using Serilog;

namespace Hub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //serilog
            builder.Host.AddSerilog();
            //masstransit
            builder.Services.AddMasstransit(builder.Configuration);
            //signal r
            builder.Services.AddSignalR();
            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.MapHub<NotificationHub>("/notifications");
            app.Run();
        }
    }
}
