
using System.Collections.Generic;

namespace BetterAnimeList.Classes
{
    public class Member
    {
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Profilepicpath { get; set; }

       
       
        public Member()
        {
           



        }

        public Member(int newid, string newusername)
        {
            Userid = newid;
            Username = newusername;
        }
    }
}
