using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace PassSync {
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
