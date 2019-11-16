using BetterAnimeList.Classes;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BetterAnimeList.Models
{

    //SignInManager
    public class AnimeDBContext : DbContext
    {
        public AnimeDBContext(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public AnimeDBContext(DbContextOptions<AnimeDBContext> options) : base(options)
        {
        }

        private string ConnectionString { get; set; }


        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        //Sets up EF Core 3.0 to work with MySQL
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
        }

        public List<Member> GetAllMembers()
        {
            var list = new List<Member>();

            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("select * from userlogin", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member()
                        {
                            Userid = Convert.ToInt32(reader["userloginid"]),
                            Username = reader["userloginname"].ToString()

                        });
                    }
                }
            }
            return list;
        }

        public Member GetMemberByUsername(string Username)
        {
            var OutMember = new Member();

            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("select * from userlogin where @username = userloginname", conn);
                cmd.Parameters.AddWithValue("@username", Username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OutMember.Userid = Convert.ToInt32(reader["userloginid"]);
                        OutMember.Username = reader["userloginname"].ToString();
                    }
                }
            }

            return OutMember;
        }


        public AnimeListViewModel GetUserAnimelist(int UserID)
        {
            var Model = new AnimeListViewModel();

            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM idincludedanimelist WHERE userid = @userid", conn);
                cmd.Parameters.AddWithValue("@userid", UserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    var list = new List<AnimeListItem>();

                    bool FirstRun = false;
                    var NewMember = new Member();
                    while (reader.Read())
                    {
                        if (!FirstRun)
                        {
                            // Create member of the list
                            NewMember.Userid = Convert.ToInt32(reader["userid"]);
                            NewMember.Username = reader["username"].ToString();

                            FirstRun = true;
                        }
                        
                        var NewItem = new AnimeListItem();

                        // Create the anime from the reader
                        var NewAnime = new Anime();
                        NewAnime.AnimeID = Convert.ToInt32(reader["animeid"]);
                        NewAnime.Title = reader["title"].ToString();
                        NewAnime.Epcount = Convert.ToInt32(reader["epcount"]);
                        NewAnime.TypeName = reader["typename"].ToString();

                        NewItem.Anime = NewAnime;
                        NewItem.Progress = Convert.ToInt32(reader["progress"]);
                        NewItem.Rating = Convert.ToInt32(reader["rating"]);
                        NewItem.StatusName = reader["statusname"].ToString();

                        list.Add(NewItem);
                    }

                    

                    // Add to our viewmodel
                    Model.AnimeListItems = list;
                    Model.Member = NewMember;
                }
            }

            // Will return empty if no results found
            return Model;
        }

        public AnimeListViewModel GetUserAnimelistByUsername(string Username)
        {
            var Model = new AnimeListViewModel();

            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM idincludedanimelist WHERE username = @username", conn);
                cmd.Parameters.AddWithValue("@username", Username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    var list = new List<AnimeListItem>();

                    bool FirstRun = false;
                    var NewMember = GetMemberByUsername(Username);

                    if (NewMember == null)
                    {
                        return Model;
                    }

                    while (reader.Read())
                    {
                        var NewItem = new AnimeListItem();

                        // Create the anime from the reader
                        var NewAnime = new Anime();
                        NewAnime.AnimeID = Convert.ToInt32(reader["animeid"]);
                        NewAnime.Title = reader["title"].ToString();
                        NewAnime.Epcount = Convert.ToInt32(reader["epcount"]);
                        NewAnime.TypeName = reader["typename"].ToString();

                        NewItem.Anime = NewAnime;
                        NewItem.Progress = Convert.ToInt32(reader["progress"]);
                        NewItem.Rating = Convert.ToInt32(reader["rating"]);
                        NewItem.StatusName = reader["statusname"].ToString();

                        list.Add(NewItem);
                    }



                    // Add to our viewmodel
                    Model.AnimeListItems = list;
                    Model.Member = NewMember;
                }
            }

            // Will return empty if no results found
            return Model;
        }

        public void UploadCSV(string filepath)
        {

            using (var conn = GetConnection())
            {


                MySqlBulkLoader bl = new MySqlBulkLoader(conn);
                bl.Local = false;
                bl.TableName = "staging_mal";
                bl.FieldTerminator = "|";
                bl.LineTerminator = "\n";
                bl.FileName = filepath;

                conn.Open();

                // Upload data from file
                int count = bl.Load();

            }
        }



        public Member register(string username, string password)
        {
            using (var conn = GetConnection())
            {
                if (doesuserexist(username))
                {
                    return null;
                }

                password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO userlogin (userloginname, password, registerdate) VALUES(@username, @password, now() )", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return new Member();
                }
            }

        }

        public Member login(string username, string password)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM userlogin WHERE userloginname = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (BCrypt.Net.BCrypt.EnhancedVerify(password, reader["password"].ToString()))
                        {
                            return new Member(Convert.ToInt32(reader["userloginid"]), reader["userloginname"].ToString());
                        }
                    }


                    return null;
                }

            }

        }


        public bool doesuserexist(string username)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM userlogin WHERE userloginname = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int RowCount = 0;
                    while (reader.Read())
                    {
                        RowCount++;
                    }
                    if (RowCount > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }









    }
}
