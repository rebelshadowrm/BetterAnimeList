using BetterAnimeList.Classes;
using System.Collections.Generic;

namespace BetterAnimeList.Models
{
    public class UserListViewModel
    {
        public List<Member> Users { get; set; }

        public UserListViewModel(List<Member> users)
        {
            Users = users;

        }
    }
}


