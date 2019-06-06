using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly string _baseUrl = "https://www.billboard.com/";

        public async Task<IEnumerable<Song>> ParseSongsAsync(string link)
        {
            var document = await _web.LoadFromWebAsync(_baseUrl + link, Encoding.UTF8).ConfigureAwait(false);

            var songsNode = document.DocumentNode.SelectNodes(ByClassName("chart-list-item"));
            var songs = new List<Song>();
            foreach (var node in songsNode)
            {
                var artist = node.SelectSingleNode("." + ByClassName("chart-list-item__artist"))?.InnerText;
                var songName = node.SelectSingleNode("." + ByClassName("chart-list-item__title-text"))?.InnerText;
                var avatarNode = node.SelectSingleNode("." + ByClassName("chart-list-item__image"));
                var lastWeek = node.SelectSingleNode("." + ByClassName("chart-list-item__last-week"))?.InnerText;
                var peakPosition = node.SelectSingleNode("." + ByClassName("chart-list-item__weeks-at-one"))?.InnerText;
                var weeksOnChart = node.SelectSingleNode("." + ByClassName("chart-list-item__weeks-on-chart"))?.InnerText;

                songs.Add(new Song
                {
                    Artist = Format(artist),
                    Name = Format(songName),
                    Avatar = Format(GetAttribute(avatarNode, "data-src")),
                    LastWeek = Format(lastWeek),
                    PeakPosition = Format(peakPosition),
                    WeeksOnChart = Format(weeksOnChart)
                });
            }

            return songs;
        }

        public async Task<IEnumerable<string>> ParseCategoriesAsync()
        {
            var url = _baseUrl + "/charts";
            var document = await _web.LoadFromWebAsync(url).ConfigureAwait(false);

            var categoriesNode = document.DocumentNode.SelectSingleNode(ByClassName("chart-panel--main"));

            var nodes = categoriesNode.SelectNodes("." + ByClassName("chart-panel__text"));

            var categories = nodes.Select(n => Format(n.InnerText));
            return categories;
        }

        public async Task<IEnumerable<Chart>> ParseChartsAsync(string category)
        {
            var url = _baseUrl + "/charts";
            var document = await _web.LoadFromWebAsync(url).ConfigureAwait(false);

            var id = GetIdForCategory(category);
            var chartsNode = document.DocumentNode.SelectSingleNode(ById(id));

            var nodes = chartsNode.SelectNodes("." + ByClassName("chart-panel__link"));
            var charts = new List<Chart>();
            foreach (var node in nodes)
            {
                var link = GetAttribute(node, "href");
                var name = node.SelectSingleNode("." + ByClassName("chart-panel__text")).InnerText;

                charts.Add(new Chart
                {
                    Name = Format(name),
                    Link = Format(link)
                });
            }

            return charts;
        }

        private string Format(string str)
        {
            return WebUtility.HtmlDecode(str?.Trim());
        }

        private string GetIdForCategory(string category)
        {
            var sb = new StringBuilder(category.ToLower());
            sb.Replace(" ", string.Empty);
            sb.Replace("/", string.Empty);
            sb.Replace("&", string.Empty);
            sb.Append("ChartPanel");

            return sb.ToString();
        }

        private string GetAttribute(HtmlNode node, string name)
        {
            return node.Attributes.FirstOrDefault(a => a.Name == name)?.Value;
        }

        private string ByClassName(string className)
        {
            return $"//*[contains(concat(' ', @class, ' '), ' {className} ')]";
        }

        private string ById(string id)
        {
            return $"//*[contains(@id, '{id}')]";
        }
    }
}