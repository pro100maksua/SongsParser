using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SongsParser.Entities;
using SongsParser.Interfaces;

namespace SongsParser.Services
{
    public class BillboardSongsParser : ISongsParser
    {
        private readonly HtmlWeb _web = new HtmlWeb();

        public async Task<IEnumerable<Song>> ParseSongsAsync(string url)
        {
            var document = await _web.LoadFromWebAsync(url, Encoding.UTF8).ConfigureAwait(false);

            var nodes = document.DocumentNode.SelectNodes(ByClassName("chart-list-item"));
            var songs = new List<Song>();
            foreach (var node in nodes)
            {
                var artist = node.SelectSingleNode($".{ByClassName("chart-list-item__artist")}").InnerText;
                var songName = node.SelectSingleNode($".{ByClassName("chart-list-item__title-text")}").InnerText;

                songs.Add(new Song
                {
                    Artist = artist.Replace("\n",string.Empty), 
                    Name = songName.Replace("\n", string.Empty) 
                });
            }

            return songs;
        }

        private string ByClassName(string className)
        {
            return $"//*[contains(concat(' ', @class, ' '), ' {className} ')]";
        }
    }
}
