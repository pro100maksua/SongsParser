using System.Collections.Generic;
using SongsParser.Entities;

namespace SongsParser.Interfaces
{
    public interface ISongsParser
    {
        IEnumerable<Song> ParseSongs(string url);
    }
}