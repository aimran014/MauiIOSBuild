using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using TamsMobile.Services;

namespace TamsMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

            // Add MudBlazor Services
            builder.Services.AddMudServices();

            // Configure a shared HttpClient with SSL bypass
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            // Register a singleton HttpClient
            builder.Services.AddSingleton(sp => new HttpClient(httpHandler)
            {
                //BaseAddress = new Uri("https://192.168.0.240:8447"),
                //BaseAddress = new Uri("https://192.168.0.245:448"),
                BaseAddress = new Uri("https://ptpacsisb.ddns.net:4433"),
                Timeout = TimeSpan.FromSeconds(30)
            });

            // Register individual services as singletons (they will receive the HttpClient from DI)
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<ProfileService>();
            builder.Services.AddSingleton<AttendanceService>();
            builder.Services.AddSingleton<ApprovalService>();
            builder.Services.AddSingleton<UrusanService>();
            builder.Services.AddSingleton<KemaskiniAttendanceService>();
            builder.Services.AddSingleton<GPSService>();
            builder.Services.AddSingleton<ReportingService>();
            builder.Services.AddSingleton<ExportService>();

            // Register session manager
            builder.Services.AddSingleton<UserSessionManager>();

            var app = builder.Build();
            
            Console.WriteLine("===== MauiApp Built Successfully =====");
            
            return app;
        }
    }
}
