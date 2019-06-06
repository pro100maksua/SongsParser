using System;

namespace SongsParser.Entities
{
    public class Song
    {
        public string Artist { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string LastWeek { get; set; }

        public string PeakPosition { get; set; }

        public string WeeksOnChart { get; set; }
    }
}
