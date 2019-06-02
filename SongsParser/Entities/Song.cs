using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsParser.Entities
{
    public class Song
    {
        public string Artist { get; set; }

        public string Name { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
