using Microsoft.Extensions.Logging;
using Plugin.Maui.Pedometer;
using StepCounter.Data;
using StepCounter.Helpers;
using StepCounter.Services;
using StepCounter.ViewModels;

namespace StepCounter
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IPedometer>(Pedometer.Default);
            builder.Services.AddSingleton<StepCounterService>();

            builder.Services.AddSingleton<Utils>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "steps.db3");
            builder.Services.AddSingleton(new StepDatabase(dbPath));

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

            builder.Services.AddTransient<HistoryViewModel>();
            builder.Services.AddTransient<HistoryPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
