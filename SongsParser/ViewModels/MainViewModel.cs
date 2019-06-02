using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SongsParser.Entities;
using SongsParser.Interfaces;
using SongsParser.Services;
using Splat;

namespace SongsParser.ViewModels
{
    public class MainViewModel: ReactiveObject
    {
        private readonly ISongsParser _songsParser;

        [Reactive] public string Url { get; set; }
        [Reactive] public IEnumerable<Song> Songs { get; set; }

        public ReactiveCommand<Unit,Unit> LoadSongsCommand { get; }

        public MainViewModel(ISongsParser songsParser = default)
        {
            _songsParser = songsParser ?? Locator.Current.GetService<ISongsParser>();

            LoadSongsCommand = ReactiveCommand.Create(LoadSongs);
        }

        private void LoadSongs()
        {
            Songs = _songsParser.ParseSongs(Url);
        }
    }
}