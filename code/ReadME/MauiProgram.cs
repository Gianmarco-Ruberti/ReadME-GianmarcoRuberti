using Microsoft.Extensions.Logging;
using ReadMe_perso.Converters;
using ReadMe_perso.Views;

namespace ReadMe_perso
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

            // Register Resources
            var resources = new ResourceDictionary();
            resources.Add("ProgressConverter", new ProgressConverter());
            Application.Current?.Resources.MergedDictionaries.Add(resources);

            // Register Pages and ViewModels
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddSingleton<ReaderPage>();
            builder.Services.AddSingleton<TagManagementPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
