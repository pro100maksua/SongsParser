using System.Reactive.Disposables;
using System.Windows.Controls;
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
                this.BindCommand(ViewModel, vm => vm.LoadSongsCommand, v => v.LoadButton)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.ExportSongsCommand, v => v.ExportButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.Error, v => v.ErrorTextBlock.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.Songs, v => v.SongsListBox.ItemsSource)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.Categories, v => v.CategoriesComboBox.ItemsSource)
                    .DisposeWith(disposable);
                this.OneWayBind(ViewModel, vm => vm.Charts, v => v.ChartsComboBox.ItemsSource)
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.CategoriesComboBox.SelectedItem)
                    .BindTo(this, x => x.ViewModel.SelectedCategory);

                this.WhenAnyValue(x => x.ChartsComboBox.SelectedItem)
                    .BindTo(this, x => x.ViewModel.SelectedChart);
                
                CategoriesComboBox.Events().DropDownOpened.InvokeCommand(ViewModel, vm => vm.LoadCategories)
                    .DisposeWith(disposable);
            });
        }
    }
}
