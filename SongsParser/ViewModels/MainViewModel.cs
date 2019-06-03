using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        [Reactive] public string Url { get; set; }
        [Reactive] public IEnumerable<Song> Songs { get; set; }
        [Reactive] public string Error { get; set; }

        public ReactiveCommand<Unit, Unit> LoadSongsCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportSongsCommand { get; }

        public MainViewModel(ISongsParser songsParser = default, ICSVService csvService = default, IBrowserService browserService = default)
        {
            _songsParser = songsParser ?? Locator.Current.GetService<ISongsParser>();
            _csvService = csvService ?? Locator.Current.GetService<ICSVService>();
            _browserService = browserService ?? Locator.Current.GetService<IBrowserService>();

            Songs = Enumerable.Empty<Song>();

            LoadSongsCommand = ReactiveCommand.CreateFromTask(LoadSongsAsync,
                this.WhenAnyValue(vm => vm.Url, url => !string.IsNullOrWhiteSpace(url)));
            ExportSongsCommand = ReactiveCommand.Create(ExportSongs,
                this.WhenAnyValue(vm => vm.Songs, s => s.Count() != 0));

            LoadSongsCommand.ThrownExceptions.Subscribe((ex) => Error = ex.Message);
            ExportSongsCommand.ThrownExceptions.Subscribe((ex) => Error = ex.Message);

            this.WhenAnyValue(vm => vm.Url)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => Error = string.Empty);
        }

        private async Task LoadSongsAsync()
        {
            Songs = await _songsParser.ParseSongsAsync(Url);
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