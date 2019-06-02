using System;

namespace SongsParser.Entities
{
    public class Song
    {
        public string Artist { get; set; }

        public string Name { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
