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
        private bool connected = false; //trạng thái kết nối
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
        private const int AuthorizationTimeoutMs = 5000;
        private const int AttachmentLimitBytes = 5 * 1024 * 1024; // 5MB

        public Client()
        {
            InitializeComponent();
        }

        //ghi log lên chatPanel

        private void Log(string msg = "")
        {
            if (!exit)
            {
                chatPanel.Invoke((MethodInvoker)delegate
                {
                    if (msg.Length > 0)
                    {
                        Label lbl = new Label();
                        lbl.Text = $"[{DateTime.Now:HH:mm}] {msg}";
                        lbl.AutoSize = true;
                        chatPanel.Controls.Add(lbl);
                    }
                    else
                    {
                        chatPanel.Controls.Clear();
                    }
                });
            }
        }


        //Trả về tin nhắn lỗi
        private string ErrorMsg(string msg) => $"LỖI: {msg}";
        //Trả về tin nhắn hệ thống
        private string SystemMsg(string msg) => $"HỆ THỐNG: {msg}";
        //Cập nhật trạng thái kết nối và ui
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
                    connectButton.Text = status ? "Ngắt kết nối" : "Kết nối";
                    Log(SystemMsg(status ? "Bạn đã kết nối" : "Bạn đã ngắt kết nối"));

                    //nếu đã ngắt kết nối thì xóa luôn userlist cũ
                    if (!status)
                    {
                        clientsDataGridView.Rows.Clear();
                    }
                });
            }
        }

        // Hiển thị tin nhắn lên chatPanel
        private void DisplayMessage(string msg)
        {
            chatPanel.Invoke((MethodInvoker)delegate
            {
                // === 1) IMAGE ===
                if (msg.StartsWith("[IMAGE]"))
                {
                    if (!TryParseAttachment(msg, out string user, out string fileName, out byte[] data, "hình ảnh"))
                    {
                        return;
                    }

                    AddSenderLabel(user);

                    Image previewImage;
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        try
                        {
                            previewImage = (Image)Image.FromStream(ms).Clone();
                        }
                        catch (Exception ex)
                        {
                            Log(ErrorMsg($"Không thể đọc hình ảnh: {ex.Message}"));
                            return;
                        }
                    }

                    PictureBox pic = new PictureBox
                    {
                        Image = previewImage,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 250,
                        Height = 180,
                        Cursor = Cursors.Hand
                    };

                    pic.Click += (s, e) => ShowImageViewer(fileName, previewImage);
                    pic.Disposed += (s, e) => pic.Image?.Dispose();

                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("Xem ảnh", null, (s, e) => ShowImageViewer(fileName, previewImage));
                    menu.Items.Add("Lưu ảnh...", null, (s, e) => SaveAttachment(data, fileName, "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*", askOpen: false));
                    pic.ContextMenuStrip = menu;

                    chatPanel.Controls.Add(pic);
                    return;
                }

                // === 2) FILE ===
                if (msg.StartsWith("[FILE]"))
                {
                    if (!TryParseAttachment(msg, out string user, out string fileName, out byte[] data, "tệp"))
                    {
                        return;
                    }

                    AddSenderLabel(user);

                    FlowLayoutPanel fileContainer = new FlowLayoutPanel
                    {
                        AutoSize = true,
                        FlowDirection = FlowDirection.LeftToRight,
                        Margin = new Padding(0, 0, 0, 5)
                    };

                    Label fileLabel = new Label
                    {
                        Text = fileName,
                        AutoSize = true,
                        Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline),
                        ForeColor = Color.Blue,
                        Cursor = Cursors.Hand,
                        Margin = new Padding(0, 6, 6, 0)
                    };
                    fileLabel.Click += (s, e) => SaveAttachment(data, fileName, "All files|*.*", askOpen: true);

                    Button downloadButton = new Button
                    {
                        Text = "Tải xuống",
                        AutoSize = true,
                        Margin = new Padding(0, 0, 6, 0)
                    };
                    downloadButton.Click += (s, e) => SaveAttachment(data, fileName, "All files|*.*", askOpen: true);

                    Label sizeLabel = new Label
                    {
                        AutoSize = true,
                        Text = $"({FormatFileSize(data.Length)})",
                        Margin = new Padding(0, 6, 0, 0)
                    };

                    fileContainer.Controls.Add(fileLabel);
                    fileContainer.Controls.Add(downloadButton);
                    fileContainer.Controls.Add(sizeLabel);
                    chatPanel.Controls.Add(fileContainer);
                    return;
                }

                // === 3) TEXT MESSAGE ===
                Label lbl = new Label
                {
                    Text = $"[{DateTime.Now:HH:mm}] {msg}",
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular)
                };
                chatPanel.Controls.Add(lbl);
            });
        }

        private bool TryDecodeBase64(string base64, string description, out byte[] data)
        {
            data = Array.Empty<byte>();
            try
            {
                data = Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException ex)
            {
                Log(ErrorMsg($"Dữ liệu {description} không hợp lệ: {ex.Message}"));
            }
            catch (Exception ex)
            {
                Log(ErrorMsg($"Không thể đọc dữ liệu {description}: {ex.Message}"));
            }
            return false;
        }

        private bool TryParseAttachment(string msg, out string user, out string fileName, out byte[] data, string description)
        {
            user = string.Empty;
            fileName = string.Empty;
            data = Array.Empty<byte>();
            string[] parts = msg.Split('|');
            if (parts.Length < 4)
            {
                Log(ErrorMsg($"Dữ liệu {description} không đúng định dạng."));
                return false;
            }
            user = parts[1];
            fileName = parts[2];
            string base64 = string.Join("|", parts, 3, parts.Length - 3);
            return TryDecodeBase64(base64, description, out data);
        }

        private void AddSenderLabel(string user)
        {
            Label userLabel = new Label
            {
                Text = $"[{DateTime.Now:HH:mm}] {user}:",
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular)
            };
            chatPanel.Controls.Add(userLabel);
        }

        private void SaveAttachment(byte[] data, string fileName, string filter, bool askOpen)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog
                {
                    FileName = fileName,
                    Filter = filter
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveDialog.FileName, data);
                        if (askOpen)
                        {
                            DialogResult open = MessageBox.Show("Tệp đã lưu. Mở ngay bây giờ?", "Mở tệp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (open == DialogResult.Yes)
                            {
                                ProcessStartInfo psi = new ProcessStartInfo(saveDialog.FileName)
                                {
                                    UseShellExecute = true
                                };
                                Process.Start(psi);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lưu tệp: {ex.Message}");
            }
        }

        private void ShowImageViewer(string fileName, Image preview)
        {
            Form viewer = new Form
            {
                Text = fileName,
                Size = new Size(600, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            PictureBox big = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = (Image)preview.Clone()
            };

            viewer.FormClosed += (s, e) => big.Image?.Dispose();
            viewer.Controls.Add(big);
            viewer.ShowDialog();
        }

        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            double size = bytes;
            int order = 0;
            while (size >= 1024 && order < suffixes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {suffixes[order]}";
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
                        obj.data.Clear();

                        HandleIncomingMessage(message); 
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
                    bool signaled = obj.handle.WaitOne(AuthorizationTimeoutMs);
                    if (!signaled)
                    {
                        Log(SystemMsg("Hết thời gian chờ xác thực"));
                        obj.client.Close();
                        break;
                    }
                    if (connected)
                    {
                        success = true;
                        break;
                    }
                }
                catch (Exception ex) { Log(ErrorMsg(ex.Message)); }
            }
            if (!connected) Log(SystemMsg("Không được xác thực"));
            return success;
        }

        private void Connection(string address, int port, string username, string key)
        {
            try
            {
                obj = new MyClient
                {
                    username = username,
                    key = key,
                    client = new TcpClient()
                };

                try
                {
                    // Thử kết nối
                    obj.client.Connect(address, port);
                }
                catch (SocketException)
                {
                    // Sai IP, sai port, server không chạy, không ping được...
                    Log(SystemMsg("Không thể kết nối đến server (địa chỉ/cổng không đúng hoặc server không chạy)."));
                    return;
                }

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
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                // Đang kết nối thì bấm nút sẽ ngắt
                obj.client.Close();
            }
            else if (client == null || !client.IsAlive)
            {
                bool error = false;
                string address = addrTextBox.Text.Trim();
                string number = portTextBox.Text.Trim();
                string username = usernameTextBox.Text.Trim();

                // 1. ĐỊA CHỈ BỎ TRỐNG
                if (string.IsNullOrWhiteSpace(address))
                {
                    error = true;
                    Log(SystemMsg("Cần nhập địa chỉ server"));
                }

                int port = 0;

                // 2. CỔNG BỎ TRỐNG
                if (string.IsNullOrWhiteSpace(number))
                {
                    error = true;
                    Log(SystemMsg("Cần nhập cổng server"));
                }
                else
                {
                    // Không bỏ trống nhưng không parse được / ngoài khoảng hợp lệ
                    if (!int.TryParse(number, out port) || port <= 0 || port > 65535)
                    {
                        error = true;
                        Log(SystemMsg("Không thể kết nối đến server (địa chỉ/cổng không đúng hoặc server không chạy)."));
                    }
                }

                // 3. USERNAME
                if (string.IsNullOrWhiteSpace(username))
                {
                    error = true;
                    Log(SystemMsg("Cần nhập tên người dùng"));
                }

                // 4. Nếu không có lỗi nhập liệu → thử kết nối
                if (!error)
                {
                    client = new Thread(() => Connection(address, port, username, keyTextBox.Text))
                    {
                        IsBackground = true
                    };
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
            menu.Items.Add("Ảnh", null, (s, ev) => SendImage());
            menu.Items.Add("Tệp", null, (s, ev) => SendFile());
            menu.Items.Add("Emoji", null, (s, ev) => SendEmoji());
            menu.Show(Cursor.Position);
        }

        //Gửi Image
        private void SendImage()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = dlg.FileName;
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > AttachmentLimitBytes)
                {
                    MessageBox.Show(
                        $"Ảnh vượt quá giới hạn {AttachmentLimitBytes / (1024 * 1024)} MB.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                string fileName = Path.GetFileName(filePath);
                byte[] data = File.ReadAllBytes(filePath);
                string base64 = Convert.ToBase64String(data);

                string msg = $"[IMAGE]|{obj.username}|{fileName}|{base64}";

                // Hiện ảnh cho chính mình
                DisplayMessage(msg);

                // Gửi lên server
                Send(msg);
            }
        }


        //Gửi file
        private void SendFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = dlg.FileName;
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > AttachmentLimitBytes)
                {
                    MessageBox.Show(
                        $"Tệp vượt quá giới hạn {AttachmentLimitBytes / (1024 * 1024)} MB.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                string fileName = Path.GetFileName(filePath);
                byte[] data = File.ReadAllBytes(filePath);
                string base64 = Convert.ToBase64String(data);

                string msg = $"[FILE]|{obj.username}|{fileName}|{base64}";

                // Hiện file cho chính mình
                DisplayMessage(msg);

                // Gửi lên server
                Send(msg);
            }
        }


        //Chọn Emoji
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

        //Gửi text khi ấn Enter
        private void SendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendTextMessage();
            }
        }
        private void SendTextMessage()
        {
            if (!connected) return;                // không gửi khi chưa connect

            string msg = sendTextBox.Text.Trim();
            if (msg.Length == 0) return;

            // Hiện lên màn hình cho chính mình
            DisplayMessage($"{obj.username} (Bạn): {msg}");

            // Gửi cho server (server sẽ forward cho client khác)
            Send($"{obj.username}: {msg}");

            sendTextBox.Clear();
        }

        private void ConnectionFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ConnectButton_Click(sender, EventArgs.Empty);
            }
        }

        //Xóa log
        private void ClearButton_Click(object sender, EventArgs e)
            {
            Log();                      // Xóa log
            chatPanel.Controls.Clear(); // Xóa toàn bộ nội dung khung chat
            }

        //Hiện thị Key
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            keyTextBox.PasswordChar = checkBox.Checked ? '*' : '\0';
        }

        //Đóng form
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit = true;
            if (connected) obj.client.Close();
        }

        private void chatPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            SendTextMessage();
        }

        private void clientsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!connected) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Chỉ xử lý khi click vào cột "Message"
            if (clientsDataGridView.Columns[e.ColumnIndex].Name == "Message")
            {
                string idStr = clientsDataGridView.Rows[e.RowIndex].Cells["identifier"].Value?.ToString();
                string targetName = clientsDataGridView.Rows[e.RowIndex].Cells["username"].Value?.ToString();

                if (!long.TryParse(idStr, out long targetId) || string.IsNullOrEmpty(targetName))
                    return;

                string text = PromptPrivateMessage(targetName);
                if (string.IsNullOrWhiteSpace(text)) return;

                // Hiện cho chính mình
                DisplayMessage($"(Riêng tới {targetName}) {obj.username}: {text}");

                // Gửi lên server
                string payload = $"{obj.username} (riêng): {text}";
                string msg = $"[PRIVATE]|{targetId}|{payload}";
                Send(msg);
            }
        }

        private void HandleIncomingMessage(string message)
        {
            // Có thể server gửi 2–3 message dính lại trong 1 lần đọc
            // Ví dụ: "HỆ THỐNG: A has disconnected[USERLIST]|..."
            int idx = message.IndexOf("[USERLIST]");
            if (idx >= 0)
            {
                // Phần trước [USERLIST] (log hệ thống, chat text,...)
                string before = message.Substring(0, idx).Trim();
                // Phần từ [USERLIST] trở đi
                string userListPart = message.Substring(idx);

                if (!string.IsNullOrEmpty(before))
                {
                    DisplayMessage(before);
                }

                // Cập nhật lại bảng user từ gói [USERLIST]
                UpdateClientGrid(userListPart);
            }
            else
            {
                // Không có [USERLIST] → chỉ là tin nhắn thường
                DisplayMessage(message);
            }
        }


        // message format: [USERLIST]|id1:username1|id2:username2|...
        private void UpdateClientGrid(string message)
        {
            string[] parts = message.Split('|');
            var rows = new List<(long id, string name)>();

            for (int i = 1; i < parts.Length; i++)
            {
                string item = parts[i];
                if (string.IsNullOrWhiteSpace(item)) continue;

                string[] kv = item.Split(new[] { ':' }, 2);
                if (kv.Length == 2 && long.TryParse(kv[0], out long id))
                {
                    string name = kv[1];

                    // nếu không muốn hiển thị chính mình:
                    if (string.Equals(name, obj.username, StringComparison.OrdinalIgnoreCase))
                        continue;

                    rows.Add((id, name));
                }
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => BindClientGrid(rows)));
            }
            else
            {
                BindClientGrid(rows);
            }
        }

        private void BindClientGrid(List<(long id, string name)> rows)
        {
            clientsDataGridView.Rows.Clear();
            foreach (var r in rows)
            {
                clientsDataGridView.Rows.Add(r.id.ToString(), r.name);
            }
        }

        private string PromptPrivateMessage(string targetName)
        {
            using (Form f = new Form())
            {
                f.Text = "Tin nhắn riêng gửi đến " + targetName;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(400, 200);

                TextBox txt = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill
                };

                Button btn = new Button
                {
                    Text = "Gửi",
                    DialogResult = DialogResult.OK,
                    Dock = DockStyle.Bottom,
                    Height = 30
                };

                f.Controls.Add(txt);
                f.Controls.Add(btn);
                f.AcceptButton = btn;

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    return txt.Text.Trim();
                }
                return string.Empty;
            }
        }



    }

}
