using System.Collections.Generic;
using System.IO;
using CsvHelper;
using SongsParser.Entities;
using SongsParser.Interfaces;
using SongsParser.Maps;

namespace SongsParser.Services
{
    public class CSVService : ICSVService
    {
        public void WriteToFile(IEnumerable<Song> songs, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<SongMap>();
                csv.WriteRecords(songs);
            }
        }
    }
}