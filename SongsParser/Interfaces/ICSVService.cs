using System.Collections.Generic;
using SongsParser.Entities;

namespace SongsParser.Interfaces
{
    public interface ICSVService
    {
        void Write(IEnumerable<Song> songs, string filePath);
    }
}