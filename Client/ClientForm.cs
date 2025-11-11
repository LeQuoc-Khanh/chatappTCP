using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

public partial class ClientForm : Form
{
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader reader;
    private StreamWriter writer;

    public ClientForm()
    {
        InitializeComponent();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            string serverAddress = txtServerAddress.Text;
            int port = int.Parse(txtPort.Text);

            // Khởi tạo TCP client
            tcpClient = new TcpClient(serverAddress, port);
            networkStream = tcpClient.GetStream();
            writer = new StreamWriter(networkStream, Encoding.ASCII) { AutoFlush = true };

            // Gửi tin nhắn
            string message = txtMessage.Text;
            writer.WriteLine(message);

            // Hiển thị tin nhắn đã gửi trong nhật ký chat
            lstChatLog.AppendText("You: " + message + "\n");

            // Nhận phản hồi từ server (nếu có)
            reader = new StreamReader(networkStream, Encoding.ASCII);
            string response = reader.ReadLine();
            lstChatLog.AppendText("Server: " + response + "\n");

            // Đóng kết nối
            writer.Close();
            reader.Close();
            networkStream.Close();
            tcpClient.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message);
        }
    }
}
