using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        private bool connected = false;
        private Thread client = null;
        private struct MyClient
        {
            public string username;
            public string key;
            public TcpClient client;
            public NetworkStream stream;
            public byte[] buffer;
            public StringBuilder data;
            public EventWaitHandle handle;
        };
        private MyClient obj;
        private Task send = null;
        private bool exit = false;

        public Client()
        {
            InitializeComponent();
        }

        
        private void Log(string msg = "")
        {
            if (!exit)
            {
                logTextBox.Invoke((MethodInvoker)delegate
                {
                    if (msg.Length > 0)
                        logTextBox.AppendText($"[{DateTime.Now:HH:mm}] {msg}{Environment.NewLine}");
                    else
                        logTextBox.Clear();
                });
            }
        }

        private string ErrorMsg(string msg) => $"ERROR: {msg}";
        private string SystemMsg(string msg) => $"SYSTEM: {msg}";

        private void Connected(bool status)
        {
            if (!exit)
            {
                connectButton.Invoke((MethodInvoker)delegate
                {
                    connected = status;
                    addrTextBox.Enabled = !status;
                    portTextBox.Enabled = !status;
                    usernameTextBox.Enabled = !status;
                    keyTextBox.Enabled = !status;
                    connectButton.Text = status ? "Disconnect" : "Connect";
                    Log(SystemMsg(status ? "You are now connected" : "You are now disconnected"));
                });
            }
        }

        
        private void DisplayMessage(string msg)
        {
            chatPanel.Invoke((MethodInvoker)delegate
            {
                if (msg.StartsWith("[IMAGE]"))
                {
                    string path = msg.Substring(7);
                    PictureBox pic = new PictureBox
                    {
                        Image = Image.FromFile(path),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 200,
                        Height = 200,
                        Cursor = Cursors.Hand
                    };
                    pic.Click += (s, e) =>
                    {
                        Form viewer = new Form
                        {
                            Text = "Xem ảnh",
                            Size = new Size(600, 600),
                            StartPosition = FormStartPosition.CenterParent
                        };
                        PictureBox bigPic = new PictureBox
                        {
                            Dock = DockStyle.Fill,
                            Image = Image.FromFile(path),
                            SizeMode = PictureBoxSizeMode.Zoom
                        };
                        viewer.Controls.Add(bigPic);
                        viewer.ShowDialog();
                    };
                    chatPanel.Controls.Add(pic);
                }
                else if (msg.StartsWith("[FILE]"))
                {
                    string filePath = msg.Substring(6);
                    Label link = new Label
                    {
                        Text = "📁 " + Path.GetFileName(filePath),
                        AutoSize = true,
                        ForeColor = Color.Blue,
                        Cursor = Cursors.Hand
                    };
                    link.Click += (s, e) =>
                    {
                        try { Process.Start(filePath); }
                        catch { MessageBox.Show("Không thể mở file."); }
                    };
                    chatPanel.Controls.Add(link);
                }
                else
                {
                    Label lbl = new Label
                    {
                        Text = msg,
                        AutoSize = true
                    };
                    chatPanel.Controls.Add(lbl);
                }
            });
        }

        
        private void Read(IAsyncResult result)
        {
            int bytes = 0;
            if (obj.client.Connected)
            {
                try { bytes = obj.stream.EndRead(result); }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
            if (bytes > 0)
            {
                obj.data.Append(Encoding.UTF8.GetString(obj.buffer, 0, bytes));
                try
                {
                    if (obj.stream.DataAvailable)
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, Read, null);
                    else
                    {
                        string message = obj.data.ToString();
                        DisplayMessage(message);
                        obj.data.Clear();
                        obj.handle.Set();
                    }
                }
                catch (Exception ex)
                {
                    obj.data.Clear();
                    Log(ErrorMsg(ex.Message));
                    obj.handle.Set();
                }
            }
            else
            {
                obj.client.Close();
                obj.handle.Set();
            }
        }

        private void ReadAuth(IAsyncResult result)
        {
            int bytes = 0;
            if (obj.client.Connected)
            {
                try { bytes = obj.stream.EndRead(result); }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
            if (bytes > 0)
            {
                obj.data.Append(Encoding.UTF8.GetString(obj.buffer, 0, bytes));
                try
                {
                    if (obj.stream.DataAvailable)
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, ReadAuth, null);
                    else
                    {
                        JavaScriptSerializer json = new JavaScriptSerializer();
                        Dictionary<string, string> data = json.Deserialize<Dictionary<string, string>>(obj.data.ToString());
                        if (data.ContainsKey("status") && data["status"].Equals("authorized"))
                            Connected(true);
                        obj.data.Clear();
                        obj.handle.Set();
                    }
                }
                catch (Exception ex)
                {
                    obj.data.Clear();
                    Log(ErrorMsg(ex.Message));
                    obj.handle.Set();
                }
            }
            else
            {
                obj.client.Close();
                obj.handle.Set();
            }
        }

        private bool Authorize()
        {
            bool success = false;
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "username", obj.username },
                { "key", obj.key }
            };
            JavaScriptSerializer json = new JavaScriptSerializer();
            Send(json.Serialize(data));
            while (obj.client.Connected)
            {
                try
                {
                    obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, ReadAuth, null);
                    obj.handle.WaitOne();
                    if (connected)
                    {
                        success = true;
                        break;
                    }
                }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
            if (!connected) Log(SystemMsg("Unauthorized"));
            return success;
        }

        private void Connection(IPAddress ip, int port, string username, string key)
        {
            try
            {
                obj = new MyClient
                {
                    username = username,
                    key = key,
                    client = new TcpClient()
                };
                obj.client.Connect(ip, port);
                obj.stream = obj.client.GetStream();
                obj.buffer = new byte[obj.client.ReceiveBufferSize];
                obj.data = new StringBuilder();
                obj.handle = new EventWaitHandle(false, EventResetMode.AutoReset);
                if (Authorize())
                {
                    while (obj.client.Connected)
                    {
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, Read, null);
                        obj.handle.WaitOne();
                    }
                    obj.client.Close();
                    Connected(false);
                }
            }
            catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                obj.client.Close();
            }
            else if (client == null || !client.IsAlive)
            {
                bool error = false;
                string address = addrTextBox.Text.Trim();
                string number = portTextBox.Text.Trim();
                string username = usernameTextBox.Text.Trim();
                IPAddress ip = null;
                try { ip = Dns.GetHostAddresses(address)[0]; }
                catch { error = true; Log(SystemMsg("Invalid address")); }

                if (!int.TryParse(number, out int port)) { error = true; Log(SystemMsg("Invalid port")); }
                if (username.Length < 1) { error = true; Log(SystemMsg("Username required")); }

                if (!error)
                {
                    client = new Thread(() => Connection(ip, port, username, keyTextBox.Text)) { IsBackground = true };
                    client.Start();
                }
            }
        }

        private void Write(IAsyncResult result)
        {
            if (obj.client.Connected)
            {
                try { obj.stream.EndWrite(result); }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
        }

        private void BeginWrite(string msg)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            if (obj.client.Connected)
            {
                try { obj.stream.BeginWrite(buffer, 0, buffer.Length, Write, null); }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
        }

        private void Send(string msg)
        {
            if (send == null || send.IsCompleted)
                send = Task.Factory.StartNew(() => BeginWrite(msg));
            else
                send.ContinueWith(_ => BeginWrite(msg));
        }

        
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!connected) return;
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Gửi ảnh", null, (s, ev) => SendImage());
            menu.Items.Add("Gửi file", null, (s, ev) => SendFile());
            menu.Items.Add("Gửi emoji", null, (s, ev) => SendEmoji());
            menu.Show(Cursor.Position);
        }

        private void SendImage()
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Ảnh|*.png;*.jpg;*.jpeg;*.bmp" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DisplayMessage("[IMAGE]" + dlg.FileName);
                Send("[IMAGE]" + dlg.FileName);
            }
        }

        private void SendFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DisplayMessage("[FILE]" + dlg.FileName);
                Send("[FILE]" + dlg.FileName);
            }
        }

        private void SendEmoji()
        {
            string[] emojis = { "😀", "😂", "😍", "😎", "😭", "😡", "👍", "❤️" };
            Form picker = new Form
            {
                Text = "Chọn Emoji",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(400, 200)
            };
            FlowLayoutPanel panel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            foreach (string emoji in emojis)
            {
                Button btn = new Button
                {
                    Text = emoji,
                    Font = new Font("Segoe UI Emoji", 16),
                    Width = 40,
                    Height = 40
                };
                btn.Click += (s, e) =>
                {
                    DisplayMessage($"{obj.username}: {emoji}");
                    Send($"{obj.username}: {emoji}");
                    picker.Close();
                };
                panel.Controls.Add(btn);
            }
            picker.Controls.Add(panel);
            picker.ShowDialog();
        }

        private void SendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                string msg = sendTextBox.Text.Trim();
                if (msg.Length > 0)
                {
                    DisplayMessage($"{obj.username} (You): {msg}");
                    Send($"{obj.username}: {msg}");
                    sendTextBox.Clear();
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e) => Log();

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            keyTextBox.PasswordChar = checkBox.Checked ? '*' : '\0';
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit = true;
            if (connected) obj.client.Close();
        }
    }
}
