using System.Collections.Generic;
using System.Threading.Tasks;
using SongsParser.Entities;

namespace SongsParser.Interfaces
{
    public interface ISongsParser
    {
        Task<IEnumerable<Song>> ParseSongsAsync(string link);
        Task<IEnumerable<string>> ParseCategoriesAsync();
        Task<IEnumerable<Chart>> ParseChartsAsync(string category);
    }
}