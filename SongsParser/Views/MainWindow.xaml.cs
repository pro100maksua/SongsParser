using System.Reactive.Disposables;
using ReactiveUI;
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

            ViewModel = new MainViewModel();

            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, vm => vm.Url, v => v.UrlTextBox.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.LoadSongsCommand, v => v.LoadButton)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.ExportSongsCommand, v => v.ExportButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.Error, v => v.ErrorTextBlock.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.Songs, v => v.SongsListBox.ItemsSource)
                    .DisposeWith(disposable);
            });
        }
    }
}
