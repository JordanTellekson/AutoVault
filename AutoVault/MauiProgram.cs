#if ANDROID
using Android.Graphics.Drawables;
#endif

using AutoVault.Services;
using Microsoft.Extensions.Logging;

namespace AutoVault
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
            builder.Services.AddSingleton<ICarRepository, CarRepository>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

#if ANDROID
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("GradientFix", (handler, view) =>
            {
                var window = handler.PlatformView?.Window;
                if (window == null)
                    return;

                var gradient = new GradientDrawable(
                    GradientDrawable.Orientation.TlBr,
                    new int[]
                    {
                        Android.Graphics.Color.ParseColor("#0f2027"),
                        Android.Graphics.Color.ParseColor("#203a43"),
                        Android.Graphics.Color.ParseColor("#2c5364")
                    }
                );

                gradient.SetCornerRadius(0f);
                window.SetBackgroundDrawable(gradient);
            });
#endif

            return builder.Build();
        }
    }
}