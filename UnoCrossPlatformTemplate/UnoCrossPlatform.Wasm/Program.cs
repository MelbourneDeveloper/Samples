using Windows.UI.Xaml;

namespace UnoCrossPlatform.Wasm
{
    public class Program
    {
        /// <summary>
        /// This is nullable but perhaps this should be initialized in the constructor?
        /// </summary>
#pragma warning disable IDE0052
        private static App? _app;
#pragma warning restore IDE0052 

        private static int Main()
        {
            Application.Start(_ => _app = new App());

            return 0;
        }
    }
}
