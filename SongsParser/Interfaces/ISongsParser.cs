using System.Collections.Generic;
using System.Threading.Tasks;
using SongsParser.Entities;

namespace SongsParser.Interfaces
{
    public interface ISongsParser
    {
        Task<IEnumerable<Song>> ParseSongsAsync(string url);
    }
}