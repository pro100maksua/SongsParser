using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SongsParser.Entities;
using SongsParser.Interfaces;

namespace SongsParser.Services
{
    public class BillboardSongsParser : ISongsParser
    {
        private readonly HtmlWeb _web;

        public BillboardSongsParser()
        {
            _web = new HtmlWeb();
        }

        public IEnumerable<Song> ParseSongs(string url)
        {
            var document = _web.Load(url);

            var nodes = document.DocumentNode.SelectNodes(ByClassName("chart-list-item"));

            var songs = new List<Song>();
            foreach (var node in nodes)
            {
                var artist = node.SelectSingleNode($".{ByClassName("chart-list-item__artist")}").InnerText;
                var songName = node.SelectSingleNode($".{ByClassName("chart-list-item__title-text")}").InnerText;

                songs.Add(new Song {Artist = artist, Name = songName});
            }

            return songs;
        }

        private string ByClassName(string className)
        {
            return $"//*[contains(concat(' ', @class, ' '), ' {className} ')]";
        }
    }
}
