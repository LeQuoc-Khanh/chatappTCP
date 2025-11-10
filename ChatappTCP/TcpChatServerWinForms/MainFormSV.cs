using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TcpChatServerWinForms
{
    public partial class MainFormSV : Form
    {
        TcpListener server;
        Thread listenThread;
        List<TcpClient> clients = new List<TcpClient>();
        bool isRunning = false;

        public MainFormSV()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                IPAddress ip = IPAddress.Parse(txtIP.Text);

                server = new TcpListener(ip, port);
                server.Start();
                isRunning = true;

                listenThread = new Thread(ListenForClients);
                listenThread.Start();

                txtLog.AppendText($"Server started at {ip}:{port}\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);
                    UpdateUserList();
                    txtLog.Invoke((MethodInvoker)(() =>
                        txtLog.AppendText("Client connected\r\n")));

                    Thread clientThread = new Thread(HandleClientComm);
                    clientThread.Start(client);
                }
                catch { break; }
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytes;

            while (isRunning)
            {
                try
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    txtLog.Invoke((MethodInvoker)(() =>
                        txtLog.AppendText("Client: " + msg + "\r\n")));

                    BroadcastMessage("Client: " + msg);
                }
                catch
                {
                    break;
                }
            }

            clients.Remove(tcpClient);
            UpdateUserList();
            tcpClient.Close();
        }

        private void BroadcastMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var c in clients.ToArray())
            {
                try
                {
                    NetworkStream ns = c.GetStream();
                    ns.Write(data, 0, data.Length);
                }
                catch
                {
                    clients.Remove(c);
                }
            }
        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            string msg = txtBroadcast.Text;
            if (msg.Trim() == "") return;

            BroadcastMessage("Server: " + msg);
            txtLog.AppendText("Server: " + msg + "\r\n");
            txtBroadcast.Clear();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isRunning = false;
            server?.Stop();
            foreach (var c in clients) c.Close();
            clients.Clear();
            UpdateUserList();
            txtLog.AppendText("Server stopped.\r\n");
        }

        private void UpdateUserList()
        {
            listUsers.Invoke((MethodInvoker)(() =>
            {
                listUsers.Items.Clear();
                for (int i = 0; i < clients.Count; i++)
                    listUsers.Items.Add("Client " + (i + 1));
            }));
        }
    }
}
