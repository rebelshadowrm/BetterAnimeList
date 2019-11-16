using BetterAnimeList.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterAnimeList.Models
{
    public class AnimeListViewModel
    {
        public List<AnimeListItem> AnimeListItems { get; set; }

        public Member Member { get; set; }

        public AnimeListViewModel()
        {
            Member = new Member();
        }

    }
}
