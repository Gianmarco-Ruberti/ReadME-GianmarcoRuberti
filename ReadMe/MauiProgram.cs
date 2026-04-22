using Microsoft.Extensions.Logging;
using ReadMe.Views;
using ReadMe.ViewModels;
using ReadMe.Services;

namespace ReadMe
{
    public static class MauiProgram
    {
        private static MauiApp _mauiApp;

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

            // Enregistrement des services
            var appDataDirectory = FileSystem.AppDataDirectory;
            builder.Services.AddSingleton(new BookService(appDataDirectory));
            builder.Services.AddSingleton(new TagService(appDataDirectory));

            // Enregistrement des pages et ViewModels
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<BookDetailPage>();
            builder.Services.AddSingleton<ReaderPage>();
            builder.Services.AddSingleton<TagsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            _mauiApp = builder.Build();
            return _mauiApp;
        }

        public static T GetService<T>() where T : class
        {
            return _mauiApp.Services.GetService(typeof(T)) as T;
        }
    }
}
