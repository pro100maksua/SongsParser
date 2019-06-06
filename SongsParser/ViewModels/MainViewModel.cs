using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using SongsParser.Entities;
using SongsParser.Interfaces;
using Splat;
using System;
using System.Reactive.Linq;

namespace SongsParser.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IBrowserService _browserService;
        private readonly ICSVService _csvService;
        private readonly ISongsParser _songsParser;
        private IEnumerable<Song> _songs;
        private string _error;
        private string _selectedCategory;
        private Chart _selectedChart;
        private IEnumerable<string> _categories;
        private IEnumerable<Chart> _charts;

        public IEnumerable<Song> Songs
        {
            get => _songs;
            set => this.RaiseAndSetIfChanged(ref _songs, value);
        } 
        public string Error
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
        }
        public Chart SelectedChart
        {
            get => _selectedChart;
            set => this.RaiseAndSetIfChanged(ref _selectedChart, value);
        }
        public IEnumerable<string> Categories
        {
            get => _categories;
            set => this.RaiseAndSetIfChanged(ref _categories, value);
        }
        public IEnumerable<Chart> Charts
        {
            get => _charts;
            set => this.RaiseAndSetIfChanged(ref _charts, value);
        }

        public ReactiveCommand<Unit, Unit> LoadSongsCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportSongsCommand { get; }
        public ReactiveCommand<EventArgs, Unit> LoadCategories { get; }

        public MainViewModel(ISongsParser songsParser = null, ICSVService csvService = null, IBrowserService browserService = null)
        {
            _songsParser = songsParser ?? Locator.Current.GetService<ISongsParser>();
            _csvService = csvService ?? Locator.Current.GetService<ICSVService>();
            _browserService = browserService ?? Locator.Current.GetService<IBrowserService>();

            Songs = Enumerable.Empty<Song>();
            Charts = Enumerable.Empty<Chart>();
            Categories = Enumerable.Empty<string>();

            LoadSongsCommand = ReactiveCommand.CreateFromTask(LoadSongsAsync,
                this.WhenAnyValue(vm => vm.SelectedChart, selector: c => c != null));
            ExportSongsCommand = ReactiveCommand.Create(ExportSongs,
                this.WhenAnyValue(vm => vm.Songs, s => s.Count() != 0));
            LoadCategories = ReactiveCommand.CreateFromTask<EventArgs>(_ => LoadCategoriesAsync());

            LoadSongsCommand.ThrownExceptions.Subscribe((ex) => Error = ex.Message);
            ExportSongsCommand.ThrownExceptions.Subscribe((ex) => Error = ex.Message);
            LoadCategories.ThrownExceptions.Subscribe((ex) => Error = ex.Message);

            this.WhenAnyValue(vm => vm.SelectedCategory)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .SelectMany(LoadChartsAsync)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(c => Charts = c);
        }

        private async Task LoadCategoriesAsync()
        {
            Categories = await _songsParser.ParseCategoriesAsync();
        }

        private async Task<IEnumerable<Chart>> LoadChartsAsync(string category)
        {
            return await _songsParser.ParseChartsAsync(category);
        }

        private async Task LoadSongsAsync()
        {
            Songs = await _songsParser.ParseSongsAsync(SelectedChart.Link);
        }

        private void ExportSongs()
        {
            try
            {
                var fileName = _browserService.BrowseFile();

                _csvService.WriteToFile(Songs, fileName);
            }
            catch (ArgumentException ex)
            {
                Error = ex.Message;
            }
        }
    }
}