using System.Windows;
using SongsParser.Interfaces;
using SongsParser.Services;
using Splat;

namespace SongsParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterConstant(new BillboardSongsParser(), typeof(ISongsParser));
            Locator.CurrentMutable.RegisterConstant(new CSVService(), typeof(ICSVService));
            Locator.CurrentMutable.RegisterConstant(new BrowserService(), typeof(IBrowserService));
        }
    }
}
