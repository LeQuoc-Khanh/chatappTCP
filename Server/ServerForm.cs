using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {
        private TcpListener? tcpListener; // Đảm bảo tcpListener có thể null
        private Thread? listenerThread; // Đảm bảo listenerThread có thể null
        private readonly List<TcpClient> clientList = new(); // Cú pháp ngắn gọn
        private bool isServerRunning = false; // Biến dừng server

        public ServerForm()
        {
            InitializeComponent();
        }

        // Khi nhấn nút Start Server
        private void BtnStart_Click(object sender, EventArgs e)
        {
            string ipAddress = txtIP.Text; // Loại bỏ khoảng trắng trước và sau chuỗi
            int port = int.Parse(txtPort.Text); // Port mà server đang lắng nghe

            try
            {
                txtLog.Clear();
                // Kiểm tra xem địa chỉ IP có hợp lệ không
                if (!IPAddress.TryParse(ipAddress, out var ip))
                {
                    MessageBox.Show("Invalid IP address format. Please enter a valid IP.");
                    return; // Dừng hàm nếu IP không hợp lệ
                }

                // Kiểm tra nếu tcpListener chưa được khởi tạo
                if (tcpListener == null)
                {
                    tcpListener = new TcpListener(ip, port);
                }

                tcpListener.Start(); // Bắt đầu server
                txtLog.AppendText("Server started...\n");

                // Kiểm tra nếu thread chưa được khởi tạo hoặc chưa chạy
                if (listenerThread == null || !listenerThread.IsAlive)
                {
                    listenerThread = new Thread(ListenForClients); // Khởi tạo thread
                    listenerThread.Start(); // Bắt đầu lắng nghe kết nối
                }

                lblStatus.Text = "Server is running..."; // Cập nhật trạng thái
                isServerRunning = true; // Đánh dấu server đang chạy
            }
            catch (Exception ex)
            {
                txtLog.AppendText("Error: " + ex.Message + "\n");
            }
        }


        // Lắng nghe kết nối từ client
        private void ListenForClients()
        {
            try
            {
                while (isServerRunning)
                {
                    // Kiểm tra nếu có kết nối mới từ client
                    if (tcpListener?.Pending() == true)
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient(); // Chấp nhận kết nối mới
                        clientList.Add(tcpClient); // Thêm client vào danh sách

                        // Tạo một thread mới để xử lý mỗi client
                        Thread clientThread = new Thread(() => HandleClient(tcpClient));
                        clientThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText("Error while listening for clients: " + ex.Message + "\n");
                }));
            }
        }

        // Xử lý mỗi client
        private void HandleClient(TcpClient tcpClient)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Thêm client vào danh sách hiển thị trong ListBox
            Invoke(new Action(() =>
            {
                lstClients.Items.Add("Client " + clientList.Count);
            }));

            try
            {
                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    txtLog.Invoke(new Action(() =>
                    {
                        txtLog.AppendText("Received: " + message + "\n");
                    }));

                    // Gửi lại tin nhắn cho tất cả các client khác
                    foreach (var client in clientList)
                    {
                        if (client != tcpClient)
                        {
                            NetworkStream stream = client.GetStream();
                            byte[] data = Encoding.ASCII.GetBytes(message);
                            stream.Write(data, 0, data.Length); // Gửi tin nhắn đến client
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText("Error in client handling: " + ex.Message + "\n");
                }));
            }
            finally
            {
                tcpClient.Close();
            }
        }

        // Khi nhấn nút Stop Server
        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                txtLog.Clear();
                isServerRunning = false; // Dừng server
                tcpListener?.Stop(); // Dừng tcpListener

                // Đảm bảo thread lắng nghe dừng an toàn
                if (listenerThread != null && listenerThread.IsAlive)
                {
                    listenerThread.Interrupt(); // Yêu cầu thread ngừng lắng nghe
                    listenerThread.Join(); // Đợi thread kết thúc hoàn toàn
                }

                // Đóng tất cả kết nối client
                foreach (var client in clientList)
                {
                    client.Close(); // Đóng kết nối của client
                }

                lblStatus.Text = "Server stopped."; // Cập nhật trạng thái
                txtLog.AppendText("Server stopped.\n"); // Ghi log khi server dừng
            }
            catch (Exception ex)
            {
                txtLog.AppendText("Error stopping the server: " + ex.Message + "\n");
            }
        }

        // Khi form đóng
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isServerRunning)
            {
                isServerRunning = false;
                tcpListener?.Stop();
                listenerThread?.Join();
            }
        }
    }
}
