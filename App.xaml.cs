using System.Configuration;
using System.Data;
using System.Windows;

namespace chatappTCP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string masterConn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
            string dbName = "ChatAppDB";
            string scriptPath = "database/Database1.sql"; // đường dẫn tương đối tới file script

            try
            {
                DatabaseInitializer.EnsureDatabaseFromScript(masterConn, dbName, scriptPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo cơ sở dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

    }

}

