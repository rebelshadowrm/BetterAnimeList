using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterAnimeList.Classes
{
    public class AnimeListItem
    {
        public Anime Anime { get; set; }

        public int Rating { get; set; }

        public int Progress { get; set; }

        public string StatusName { get; set; }

    }
}
