using MySql.Data.MySqlClient;

namespace Server
{
    public class Database
    {
        private static string connStr = "server=127.0.0.1; user=root; password=123456; database=chatappdb;";

        public static bool CheckLogin(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT id FROM users WHERE username=@u AND password=@p LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                return cmd.ExecuteScalar() != null;
            }
        }

        public static bool Register(string username, string password, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string check = "SELECT id FROM users WHERE username=@u LIMIT 1";
                MySqlCommand ck = new MySqlCommand(check, conn);
                ck.Parameters.AddWithValue("@u", username);
                if (ck.ExecuteScalar() != null) return false;

                string sql = "INSERT INTO users(username, password, email) VALUES(@u,@p,@e)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }
}
