using GrpcService.Services;

namespace GrpcService
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddSingleton<MessageRepository>()
                .AddSingleton<MessageService>()
                .AddGrpc();

            WebApplication app = builder.Build();
            app.MapGrpcService<MessageGrpcService>();
            app.Run();
        }
    }
}