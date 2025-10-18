using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace chatappTCP
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseFromScript(string masterConnectionString, string targetDbName, string scriptRelativePath)
        {
            // 1. Nếu DB đã tồn tại -> bỏ qua
            if (DatabaseExists(masterConnectionString, targetDbName))
                return;

            // 2. Tạo database mới
            using (var conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"CREATE DATABASE [{targetDbName}]";
                    cmd.ExecuteNonQuery();
                }
            }

            // 3. Đọc file script
            var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptRelativePath.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(scriptPath))
                throw new FileNotFoundException("Không tìm thấy file SQL script", scriptPath);

            var script = File.ReadAllText(scriptPath);

            // Tách batch theo GO
            var batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var targetConnStr = masterConnectionString.Replace("Initial Catalog=master", $"Initial Catalog={targetDbName}");
            using (var conn = new SqlConnection(targetConnStr))
            {
                conn.Open();
                foreach (var batch in batches)
                {
                    var sql = batch.Trim();
                    if (string.IsNullOrEmpty(sql)) continue;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 600;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static bool DatabaseExists(string masterConnectionString, string dbName)
        {
            using (var conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name=@name";
                    cmd.Parameters.AddWithValue("@name", dbName);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
    }
}
