using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RestEase;
using SongsParser.ApiInterfaces;
using SongsParser.Entities;
using SongsParser.Interfaces;

namespace SongsParser.Services
{
    public class BillboardSongsParser : ISongsParser
    {
        private readonly HtmlWeb _web = new HtmlWeb();
        private readonly IBillboardChartsApi _billboardChartsApi;

        private const string BaseUrl = "https://www.billboard.com/";


        public BillboardSongsParser()
        {
            _billboardChartsApi = RestClient.For<IBillboardChartsApi>(BaseUrl);
        }

        public async Task<IEnumerable<Song>> ParseSongsAsync(string link)
        {
            var document = await _web.LoadFromWebAsync(BaseUrl + link, Encoding.UTF8).ConfigureAwait(false);

            var songNodes = document.DocumentNode.SelectNodes(ByClassName("o-chart-results-list-row"));
            var songs = new List<Song>();
            foreach (var node in songNodes)
            {
                var artist = node.SelectSingleNode("." + ByClassName("c-label"))?.InnerText;
                var songName = node.SelectSingleNode("." + ByClassName("c-title"))?.InnerText;
                var avatarNode = node.SelectSingleNode("." + ByClassName("c-lazy-image__img"));
                var lastWeek = node.SelectSingleNode("." + ByClassName("chart-list-item__last-week"))?.InnerText;
                var peakPosition = node.SelectSingleNode("." + ByClassName("chart-list-item__weeks-at-one"))?.InnerText;
                var weeksOnChart = node.SelectSingleNode("." + ByClassName("chart-list-item__weeks-on-chart"))?.InnerText;

                songs.Add(new Song
                {
                    Artist = Format(artist),
                    Name = Format(songName),
                    Avatar = Format(GetAttribute(avatarNode, "src")),
                    LastWeek = Format(lastWeek),
                    PeakPosition = Format(peakPosition),
                    WeeksOnChart = Format(weeksOnChart),
                });
            }

            return songs;
        }

        public async Task<IEnumerable<string>> ParseCategoriesAsync()
        {
            var url = BaseUrl + "/charts";
            var document = await _web.LoadFromWebAsync(url).ConfigureAwait(false);

            var xpath = $"{ByClassName("all-charts-list")}{ByClassName("o-nav__list")}{ByClassName("c-link")}";
            var nodes = document.DocumentNode.SelectNodes(xpath);

            var categories = nodes.Select(n => Format(n.InnerText)).ToList();
            return categories;
        }

        public async Task<IEnumerable<Chart>> ParseChartsAsync(string category)
        {
            var result = await _billboardChartsApi.GetChartsHtmlAsync(category).ConfigureAwait(false);

            var document = new HtmlDocument();
            document.LoadHtml(result.Html);

            var xpath = $"{ByClassName("o-chart-list-card")}/a";
            var nodes = document.DocumentNode.SelectNodes(xpath);

            var charts = new List<Chart>();
            foreach (var node in nodes)
            {
                var link = GetAttribute(node, "href");
                var name = node.SelectSingleNode($".{ByClassName("c-span")}").InnerText;

                charts.Add(new Chart
                {
                    Name = Format(name),
                    Link = link,
                });
            }

            return charts;
        }

        private string Format(string str)
        {
            return WebUtility.HtmlDecode(str?.Trim());
        }

        private string GetAttribute(HtmlNode node, string name)
        {
            return node.Attributes.FirstOrDefault(a => a.Name == name)?.Value;
        }

        private string ByClassName(string className)
        {
            return $"//*[contains(concat(' ', @class, ' '), ' {className} ')]";
        }
    }
}