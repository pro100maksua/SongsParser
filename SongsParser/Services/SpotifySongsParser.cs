using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SongsParser.Entities;
using SongsParser.Interfaces;

namespace SongsParser.Services
{
    public class SpotifySongsParser : ISongsParser
    {
        private readonly HtmlWeb _web = new HtmlWeb();
        private readonly string _baseUrl = "https://open.spotify.com/";

        public Task<IEnumerable<Song>> ParseSongsAsync(string link)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<string>> ParseCategoriesAsync()
        {
            var url = _baseUrl + "/browse/featured";
            var document = await _web.LoadFromWebAsync(url).ConfigureAwait(false);

            var categoriesNode = document.DocumentNode.SelectSingleNode(ByClassName("content"));

            var nodes = categoriesNode.SelectNodes("." + ByClassName("_1Fj-rlIZXTSNgYOCuLh7xo"));

            var categories = nodes.Select(n => Format(n.InnerText));
            return categories;
        }

        public Task<IEnumerable<Chart>> ParseChartsAsync(string category)
        {
            throw new System.NotImplementedException();
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

        private string ById(string id)
        {
            return $"//*[contains(@id, '{id}')]";
        }
    }
}