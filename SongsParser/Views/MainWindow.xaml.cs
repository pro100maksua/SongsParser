using ReactiveUI;
using SongsParser.Services;
using SongsParser.ViewModels;

namespace SongsParser.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
