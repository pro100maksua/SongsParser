using System.Collections.Generic;
using SongsParser.Entities;

namespace SongsParser.Interfaces
{
    public interface ICSVService
    {
        void WriteToFile(IEnumerable<Song> songs, string filePath);
    }
}