using CsvHelper.Configuration;
using SongsParser.Entities;

namespace SongsParser.Maps
{
    sealed class SongMap : ClassMap<Song>
    {
        public SongMap()
        {
            AutoMap();
            Map(s => s.Duration).Optional();
        }
    }
}
