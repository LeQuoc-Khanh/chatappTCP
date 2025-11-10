using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class ServerForm : Form
    {
        private TcpListener tcpListener;
        private Thread listenerThread;
        private List<TcpClient> clientList = new List<TcpClient>();
        private List<string> clientNames = new List<string>();

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            // B?t ð?u server
            string ipAddress = txtIP.Text;
            int port = int.Parse(txtPort.Text);

            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start();
                txtLog.AppendText("Server started...\n");
                listenerThread = new Thread(ListenForClients);
                listenerThread.Start();
            }
            catch (Exception ex)
            {
                txtLog.AppendText("Error: " + ex.Message + "\n");
            }
        }

        private void ListenForClients()
        {
            while (true)
            {
                // L?ng nghe k?t n?i m?i
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                clientList.Add(tcpClient);

                // T?o m?t thread ð? x? l? m?i client
                Thread clientThread = new Thread(() => HandleClient(tcpClient));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            string clientName = "Client" + clientList.Count; // Tên m?c ð?nh cho client

            // Thêm client vào DataGridView
            Invoke(new Action(() =>
            {
                dataGridView1.Rows.Add(clientName, "Disconnect");
                clientNames.Add(clientName);
            }));

            while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText(clientName + ": " + message + "\n");
                }));

                // G?i l?i tin nh?n ð?n t?t c? client khác
                foreach (var client in clientList)
                {
                    if (client != tcpClient)
                    {
                        NetworkStream stream = client.GetStream();
                        byte[] data = Encoding.ASCII.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        private void btnSendBroadcast_Click(object sender, EventArgs e)
        {
            string broadcastMessage = txtBroadcast.Text;

            // G?i tin nh?n ð?n t?t c? các client
            foreach (var client in clientList)
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(broadcastMessage);
                stream.Write(data, 0, data.Length);
            }

            txtLog.AppendText("Server (Broadcast): " + broadcastMessage + "\n");
            txtBroadcast.Clear();
        }

        private void DisAll_Click(object sender, EventArgs e)
        {
            // Ng?t k?t n?i t?t c? client
            foreach (var client in clientList)
            {
                client.Close();
            }

            clientList.Clear();
            dataGridView1.Rows.Clear();
            txtLog.AppendText("Disconnected all clients.\n");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1) // N?u nh?n nút Disconnect
            {
                string clientName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                int clientIndex = clientNames.IndexOf(clientName);

                if (clientIndex != -1)
                {
                    clientList[clientIndex].Close();
                    clientList.RemoveAt(clientIndex);
                    clientNames.RemoveAt(clientIndex);
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                    txtLog.AppendText(clientName + " disconnected.\n");
                }
            }
        }
    }
}
