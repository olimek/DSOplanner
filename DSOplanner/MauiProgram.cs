using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
#endif

namespace DSOplanner
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
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(w =>
                    {
                        w.OnWindowCreated(window =>
                        {
                            var hwnd = WindowNative.GetWindowHandle(window);
                            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                            var appWindow = AppWindow.GetFromWindowId(windowId);

                            if (appWindow != null)
                            {
                                // Pobranie dostępnego ekranu
                                var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
                                if (displayArea != null)
                                {
                                    var workArea = displayArea.WorkArea;
                                    appWindow.MoveAndResize(new RectInt32(workArea.X, workArea.Y, workArea.Width, workArea.Height));
                                }
                            }
                        });
                    });
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
