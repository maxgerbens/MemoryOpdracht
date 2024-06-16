using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using MemoryGame.Models;

namespace MemoryGame.Data
{
    public class DataAccess
    {
        private static SqlConnectionStringBuilder _builder;
        public DataAccess()
        {
            try
            {
                //Data Source = localhost\SQLEXPRESS02; Initial Catalog = MemoryOpdracht; Integrated Security = True; Encrypt = True; Trust Server Certificate = True
                _builder = new SqlConnectionStringBuilder();
                _builder.DataSource = "localhost\\SQLEXPRESS02";
                _builder.IntegratedSecurity = true;
                _builder.ConnectTimeout = 30;
                _builder.Password = "";
                _builder.InitialCatalog = "MemoryOpdracht";
                _builder.Encrypt = true;
                _builder.TrustServerCertificate = true;
                _builder.ApplicationIntent = ApplicationIntent.ReadWrite;
                _builder.MultiSubnetFailover = false;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void InsertPlayer(string playerName)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO dbo.Player (name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", playerName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetPlayer(string playerName)
        {
            int playerId = 0;
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.Player WHERE name = @Name";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", playerName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playerId = reader.GetInt32(0);
                        }
                    }
                }
            }

            return playerId;
        }

        public static bool CheckIfIsHighscore(int amount, string playerName, int score)
        {
            bool isInserted;
            List<int> scores = new List<int>();
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.HighScore";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scores.Add(reader.GetInt32(1));
                            if (scores.Count != 0)
                                scores = new List<int>(scores.OrderByDescending(c => c));
                        }
                    }
                }
            }

            if (scores.Count < 10)
            {
                InsertPlayer(playerName);
                InsertHighScore(amount, playerName, score);
                isInserted = true;
            }
            else
            {
                int lowestScore = scores.Last();
                if (lowestScore < score)
                {
                    RemoveHighScore(lowestScore);
                    InsertPlayer(playerName);
                    InsertHighScore(amount, playerName, score);
                    isInserted = true;
                }
                else
                {
                    isInserted = false;
                }
            }

            return isInserted;
        }

        public static void InsertHighScore(int amount, string playerName, int score)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sqlInsert = "INSERT INTO dbo.HighScore (score, playerid, amountcards) VALUES (@Score, @PlayerId, @AmountCards)";
                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    command.Parameters.AddWithValue("@Score", score);
                    command.Parameters.AddWithValue("@PlayerId", GetPlayer(playerName));
                    command.Parameters.AddWithValue("@AmountCards", amount);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void RemoveHighScore(int score)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sqlInsert = "DELETE FROM dbo.HighScore WHERE score = @Score";
                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    command.Parameters.AddWithValue("@Score", score);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public static ObservableCollection<Player> GetHighscores()
        {
            ObservableCollection<Player> players = new ObservableCollection<Player>();
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.HighScore LEFT JOIN dbo.Player ON dbo.HighScore.playerid = dbo.Player.id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(5);
                            int score = reader.GetInt32(1);
                            int amount = reader.GetInt32(3);
                            players.Add(new Player(name, score, amount));
                        }
                    }
                }
                players = new ObservableCollection<Player>(players.OrderByDescending(s => s.HighScore));
                int postion = 1;
                foreach (var p in players)
                {
                    p.Position = postion;
                    postion++;
                }
            }

            return players;
        }

        public static Image[] GetImages()
        {
            List<Image> images = new List<Image>();
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.Images";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] bytesArray = (byte[])reader.GetSqlBinary(1);
                            BitmapImage bitmapImage = new BitmapImage();

                            using (MemoryStream stream = new MemoryStream(bytesArray))
                            {
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = stream;
                                bitmapImage.EndInit();
                                bitmapImage.Freeze();
                            }

                            images.Add(new Image(bytesArray, bitmapImage));
                        }
                    }
                }
            }

            return images.ToArray();
        }
        
        public static void InsertImage(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string hexRepresentation = $"0x{BitConverter.ToString(imageBytes).Replace("-", "")}";

            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO dbo.Images ([image]) VALUES (CONVERT(VARBINARY(MAX), @ImageHexString, 1))";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ImageHexString", hexRepresentation);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteImage(byte[] imageBytes)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "DELETE FROM dbo.Images WHERE image = @Image";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Image", imageBytes);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
