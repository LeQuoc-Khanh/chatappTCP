using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        private bool active = false;
        private Thread listener = null;
        private long id = 0;
        private struct MyClient
        {
            public long id;
            public StringBuilder username;
            public TcpClient client;
            public NetworkStream stream;
            public byte[] buffer;
            public StringBuilder data;
            public EventWaitHandle handle;
        };
        private ConcurrentDictionary<long, MyClient> clients = new ConcurrentDictionary<long, MyClient>();
        private Task send = null;
        private Thread disconnect = null;
        private bool exit = false;
        private TcpClient tcpClient;

        public Server()
        {
            InitializeComponent();
        }

        private void Log(string msg)
        {
            if (exit || string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            chatPanel.Invoke((MethodInvoker)delegate
            {
                
                int maxWidth = Math.Max(50, chatPanel.ClientSize.Width - 25);

                Label logEntry = new Label
                {
                    AutoSize = true,
                    MaximumSize = new Size(maxWidth, 0),
                    Text = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm"), msg),
                };

                chatPanel.Controls.Add(logEntry);
                chatPanel.ScrollControlIntoView(logEntry);
            });
        }

        private string ErrorMsg(string msg)
        {
            return string.Format("LỖI: {0}", msg);
        }

        private string SystemMsg(string msg)
        {
            return string.Format("HỆ THỐNG: {0}", msg);
        }

        private void Active(bool status)
        {
            if (!exit)
            {
                startButton.Invoke((MethodInvoker)delegate
                {
                    active = status;
                    if (status)
                    {
                        addrTextBox.Enabled = false;
                        portTextBox.Enabled = false;
                        usernameTextBox.Enabled = false;
                        keyTextBox.Enabled = false;
                        startButton.Text = "Dừng";
                        Log(SystemMsg("Server đã khởi động"));
                    }
                    else
                    {
                        addrTextBox.Enabled = true;
                        portTextBox.Enabled = true;
                        usernameTextBox.Enabled = true;
                        keyTextBox.Enabled = true;
                        startButton.Text = "Khởi động";
                        Log(SystemMsg("Server đã dừng"));
                    }
                });
            }
        }

        private void AddToGrid(long id, string name)
        {
            if (!exit)
            {
                clientsDataGridView.Invoke((MethodInvoker)delegate
                {
                    string[] row = new string[] { id.ToString(), name };
                    clientsDataGridView.Rows.Add(row);
                    totalLabel.Text = string.Format("Tổng số client: {0}", clientsDataGridView.Rows.Count);
                });
            }
        }

        private void RemoveFromGrid(long id)
        {
            if (!exit)
            {
                clientsDataGridView.Invoke((MethodInvoker)delegate
                {
                    foreach (DataGridViewRow row in clientsDataGridView.Rows)
                    {
                        if (row.Cells["identifier"].Value.ToString() == id.ToString())
                        {
                            clientsDataGridView.Rows.RemoveAt(row.Index);
                            break;
                        }
                    }
                    totalLabel.Text = string.Format("Tổng số client: {0}", clientsDataGridView.Rows.Count);
                });
            }
        }

        private void Read(IAsyncResult result)
        {
            MyClient obj = (MyClient)result.AsyncState;
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
                    {
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, Read, obj);
                    }
                    else
                    {
                        string rawMsg = obj.data.ToString();
                        obj.data.Clear();
                        try
                        {
                            if (rawMsg.StartsWith("[IMAGE]"))
                            {
                                string logMsg = $"{obj.username} đã gửi một hình ảnh.";
                                DisplayAttachment(rawMsg);
                                Log(logMsg);
                                Send(rawMsg, obj.id); // broadcast
                            }
                            else if (rawMsg.StartsWith("[FILE]"))
                            {
                                string logMsg = $"{obj.username} đã gửi một tệp.";
                                DisplayAttachment(rawMsg);
                                Log(logMsg);
                                Send(rawMsg, obj.id);
                            }
                            else if (rawMsg.StartsWith("[PRIVATE]"))
                            {
                                HandlePrivateMessage(rawMsg, obj);
                            }
                            else
                            {
                                string logMsg = rawMsg; // đã là "username: message"
                                Log(logMsg);
                                Send(rawMsg, obj.id);
                            }

                            obj.handle.Set();
                        }
                        catch (Exception ex)
                        {
                            Log(ErrorMsg(ex.Message));
                            obj.handle.Set();
                        }
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
            MyClient obj = (MyClient)result.AsyncState;
            int bytes = 0;
            if (obj.client.Connected)
            {
                try
                {
                    bytes = obj.stream.EndRead(result);
                }
                catch (Exception ex)
                {
                    Log(ErrorMsg(ex.Message));
                }
            }
            if (bytes > 0)
            {
                obj.data.AppendFormat("{0}", Encoding.UTF8.GetString(obj.buffer, 0, bytes));
                try
                {
                    if (obj.stream.DataAvailable)
                    {
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, new AsyncCallback(ReadAuth), obj);
                    }
                    else
                    {
                        JavaScriptSerializer json = new JavaScriptSerializer();
                        Dictionary<string, string> data = json.Deserialize<Dictionary<string, string>>(obj.data.ToString());
                        if (!data.ContainsKey("username") || data["username"].Length < 1 || !data.ContainsKey("key") || !data["key"].Equals(keyTextBox.Text))
                        {
                            obj.client.Close();
                        }
                        else
                        {
                            obj.username.Append(data["username"].Length > 200 ? data["username"].Substring(0, 200) : data["username"]);
                            Send("{\"status\": \"authorized\"}", obj);
                        }
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

        private bool Authorize(MyClient obj)
        {
            bool success = false;
            while (obj.client.Connected)
            {
                try
                {
                    obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, new AsyncCallback(ReadAuth), obj);
                    obj.handle.WaitOne();
                    if (obj.username.Length > 0)
                    {
                        success = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Log(ErrorMsg(ex.Message));
                }
            }
            return success;
        }

        private void Connection(MyClient obj)
        {
            if (Authorize(obj))
            {
                tcpClient = obj.client;
                clients.TryAdd(obj.id, obj);
                AddToGrid(obj.id, obj.username.ToString());
                string msg = string.Format("{0} đã kết nối", obj.username);
                Log(SystemMsg(msg));
                Send(SystemMsg(msg), obj.id);

                BroadcastUserList();
                while (obj.client.Connected)
                {
                    try
                    {
                        obj.stream.BeginRead(obj.buffer, 0, obj.buffer.Length, new AsyncCallback(Read), obj);
                        obj.handle.WaitOne();
                    }
                    catch (Exception ex)
                    {
                        Log(ErrorMsg(ex.Message));
                    }
                }
                obj.client.Close();
                if (clients.TryRemove(obj.id, out MyClient tmp))
                {
                    RemoveFromGrid(tmp.id);

                    msg = string.Format("{0} đã ngắt kết nối", tmp.username);
                    Log(SystemMsg(msg));
                    Send(SystemMsg(msg), tmp.id);   // nếu muốn client cũng thấy SYSTEM:

                    BroadcastUserList();
                }
            }
        }

        private void Listener(IPAddress ip, int port)
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(ip, port);
                listener.Start();
                Active(true);
                while (active)
                {
                    if (listener.Pending())
                    {
                        try
                        {
                            MyClient obj = new MyClient();
                            obj.id = id;
                            obj.username = new StringBuilder();
                            obj.client = listener.AcceptTcpClient();
                            obj.stream = obj.client.GetStream();
                            obj.buffer = new byte[obj.client.ReceiveBufferSize];
                            obj.data = new StringBuilder();
                            obj.handle = new EventWaitHandle(false, EventResetMode.AutoReset);
                            Thread th = new Thread(() => Connection(obj))
                            {
                                IsBackground = true
                            };
                            th.Start();
                            id++;
                        }
                        catch (Exception ex)
                        {
                            Log(ErrorMsg(ex.Message));
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                Active(false);
            }
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
            finally
            {
                if (listener != null)
                {
                    listener.Server.Close();
                }
            }
        }
        //check IP/Port trước khi start
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (active)
            {
                // ĐANG CHẠY → STOP SERVER
                active = false;

                // Thông báo cho tất cả client: server đã dừng, rồi ngắt kết nối
                Disconnect(-1, "Server đã dừng");
            }
            else if (listener == null || !listener.IsAlive)
            {
                string address = addrTextBox.Text.Trim();
                string number = portTextBox.Text.Trim();
                string username = usernameTextBox.Text.Trim();
                bool error = false;
                IPAddress ip = null;
                if (address.Length < 1)
                {
                    error = true;
                    Log(SystemMsg("Cần nhập địa chỉ"));
                }
                else if (!TryResolveIPv4(address, out ip))
                {
                    error = true;
                    Log(SystemMsg("Địa chỉ không hợp lệ hoặc không phải IPv4"));
                }
                int port = -1;
                if (number.Length < 1)
                {
                    error = true;
                    Log(SystemMsg("Cần nhập số cổng"));
                }
                else if (!int.TryParse(number, out port))
                {
                    error = true;
                    Log(SystemMsg("Số cổng không hợp lệ"));
                }
                else if (port < 0 || port > 65535)
                {
                    error = true;
                    Log(SystemMsg("Số cổng nằm ngoài phạm vi cho phép"));
                }
                if (username.Length < 1)
                {
                    error = true;
                    Log(SystemMsg("Cần nhập tên người dùng"));
                }
                if (!error)
                {
                    listener = new Thread(() => Listener(ip, port))
                    {
                        IsBackground = true
                    };
                    listener.Start();
                }
            }
        }

        private void Write(IAsyncResult result)
        {
            MyClient obj = (MyClient)result.AsyncState;

            try
            {
                // Có thể check thêm null/Connected cho chắc
                if (obj.client != null && obj.client.Connected && obj.stream != null)
                {
                    obj.stream.EndWrite(result);
                }
            }
            catch (ObjectDisposedException)
            {
                // Stream/socket đã bị đóng trong lúc callback chạy → bỏ qua, không log lỗi
            }
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
        }

        private void BeginWrite(string msg, MyClient obj)
        {
            if (obj.client == null || obj.stream == null) return;
            if (!obj.client.Connected || !obj.stream.CanWrite) return;

            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            try
            {
                obj.stream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(Write), obj);
            }
            catch (ObjectDisposedException)
            {
                // socket đã đóng, bỏ qua
            }
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
        }

        private void BeginWrite(string msg, long id = -1) // send the message to everyone except the sender or set ID to lesser than zero to send to everyone
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            foreach (KeyValuePair<long, MyClient> obj in clients)
            {
                if (id != obj.Value.id && obj.Value.client.Connected)
                {
                    try
                    {
                        obj.Value.stream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(Write), obj.Value);
                    }
                    catch (ObjectDisposedException)
                    {
                        // socket đã đóng, bỏ qua
                    }
                    catch (Exception ex)
                    {
                        Log(ErrorMsg(ex.Message));
                    }
                }
            }
        }

        private void Send(string msg, MyClient obj)
        {
            // server stop rồi thì không gửi nữa
            if (!active) return;

            if (send == null || send.IsCompleted)
            {
                send = Task.Factory.StartNew(() => BeginWrite(msg, obj));
            }
            else
            {
                send.ContinueWith(_ => BeginWrite(msg, obj));
            }
        }

        private void Send(string msg, long id = -1)
        {
            // server stop rồi thì không gửi nữa
            if (!active) return;

            if (send == null || send.IsCompleted)
            {
                send = Task.Factory.StartNew(() => BeginWrite(msg, id));
            }
            else
            {
                send.ContinueWith(_ => BeginWrite(msg, id));
            }
        }
        private void SendServerMessage()
        {
            // 1. Server chưa chạy thì không cho gửi
            if (!active)
            {
                Log(SystemMsg("Không thể gửi tin nhắn: server chưa chạy."));
                return;
            }

            // 2. Không có client nào online thì cũng không gửi
            if (clients.IsEmpty)
            {
                Log(SystemMsg("Không thể gửi tin nhắn: không có client nào đang kết nối."));
                return;
            }

            // 3. Có server đang chạy & có client → gửi bình thường
            string msg = sendTextBox.Text.Trim();
            if (msg.Length == 0) return;

            sendTextBox.Clear();

            string user = usernameTextBox.Text.Trim();
            Log(string.Format("{0} (Bạn): {1}", user, msg));
            Send(string.Format("{0}: {1}", user, msg));
        }

        private void SendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendServerMessage();
            }
        }

        private void ConnectionFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                StartButton_Click(sender, EventArgs.Empty);
            }
        }
        private void ClientsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == clientsDataGridView.Columns["dc"].Index)
            {
                long.TryParse(clientsDataGridView.Rows[e.RowIndex].Cells["identifier"].Value.ToString(), out long id);
                Disconnect(id, "Bạn đã bị ngắt kết nối bởi server");
            }
            if (e.RowIndex >= 0 && e.ColumnIndex == clientsDataGridView.Columns["Message"].Index)
            {
                // Lấy id + username của client được chọn
                string idStr = clientsDataGridView.Rows[e.RowIndex].Cells["identifier"].Value?.ToString();
                string targetName = clientsDataGridView.Rows[e.RowIndex].Cells["username"].Value?.ToString();

                if (!long.TryParse(idStr, out long clientId) || string.IsNullOrEmpty(targetName))
                    return;

                // Mở form nhập tin nhắn riêng
                string message = PromptPrivateMessage(targetName);
                if (string.IsNullOrWhiteSpace(message))
                    return;

                // Gửi tin nhắn riêng
                SendMessageToClient(message, clientId);

                // Log thêm cho dễ nhìn
                Log($"(Private to {targetName}) Server (Bạn): {message}");
            }
        }
        // Phương thức gửi tin nhắn cho client
        private void SendMessageToClient(string message, long clientId)
        {
            if (clients.TryGetValue(clientId, out MyClient client))
            {
                string finalMsg = $"Server (riêng): {message}";

                byte[] buffer = Encoding.UTF8.GetBytes(finalMsg);
                client.stream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(Write), client);
            }
        }


        // Ngắt kết nối 1 client (id >= 0) hoặc tất cả (id < 0)
        // reason: thông báo SYSTEM gửi cho client trước khi đóng kết nối
        private void Disconnect(long id = -1, string reason = null)
        {
            if (disconnect == null || !disconnect.IsAlive)
            {
                disconnect = new Thread(() =>
                {
                    // Đá 1 client cụ thể
                    if (id >= 0)
                    {
                        if (clients.TryRemove(id, out MyClient obj))
                        {
                            // 1. Gửi thông báo riêng cho client bị đá (GHI ĐỒNG BỘ)
                            if (!string.IsNullOrEmpty(reason) &&
                                obj.client != null &&
                                obj.stream != null &&
                                obj.client.Connected)
                            {
                                try
                                {
                                    string sys = SystemMsg(reason);
                                    byte[] buf = Encoding.UTF8.GetBytes(sys);
                                    obj.stream.Write(buf, 0, buf.Length); // ghi sync để chắc chắn client nhận
                                }
                                catch { }
                            }

                            // 2. Đóng kết nối tới client đó
                            try { obj.client.Close(); } catch { }

                            // 3. Xóa khỏi grid
                            RemoveFromGrid(obj.id);

                            // 4. Log + thông báo cho các client khác
                            string msg = $"{obj.username} đã bị ngắt kết nối bởi server";
                            Log(SystemMsg(msg));              // log trên server
                            Send(SystemMsg(msg), obj.id);     // gửi cho TẤT CẢ TRỪ thằng bị đá

                            // 5. Cập nhật lại userlist
                            BroadcastUserList();
                        }
                    }
                    // Đá TẤT CẢ client
                    else
                    {
                        foreach (var kvp in clients)
                        {
                            if (clients.TryRemove(kvp.Key, out MyClient obj))
                            {
                                // Gửi thông báo riêng cho từng client (server stop / disconnect all)
                                if (!string.IsNullOrEmpty(reason) &&
                                    obj.client != null &&
                                    obj.stream != null &&
                                    obj.client.Connected)
                                {
                                    try
                                    {
                                        string sys = SystemMsg(reason);
                                        byte[] buf = Encoding.UTF8.GetBytes(sys);
                                        obj.stream.Write(buf, 0, buf.Length);
                                    }
                                    catch { }
                                }

                                try { obj.client.Close(); } catch { }
                                RemoveFromGrid(obj.id);
                            }
                        }

                        // Sau khi clear hết → gửi lại userlist (lúc này hầu như rỗng)
                        BroadcastUserList();
                    }
                })
                {
                    IsBackground = true
                };
                disconnect.Start();
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            // Đang run server nhưng admin muốn đá hết client
            Disconnect(-1, "Bạn đã bị ngắt kết nối bởi server");
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit = true;
            active = false;

            // Thông báo cho tất cả client: server đã dừng
            Disconnect(-1, "Server đã dừng");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            chatPanel.Controls.Clear();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            keyTextBox.PasswordChar = checkBox.Checked ? '*' : '\0';
        }

        private void clientsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private bool TryResolveIPv4(string host, out IPAddress ipAddress)
        {
            ipAddress = null;
            if (string.IsNullOrWhiteSpace(host))
            {
                return false;           
            }
            // Bắt buộc phải đúng 4 phần ngăn bởi dấu chấm
            string[] parts = host.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (string part in parts)
            {
                // Mỗi phần phải là số
                if (!int.TryParse(part, out int octet))
                    return false;

                // Mỗi octet phải nằm trong [0,255]
                if (octet < 0 || octet > 255)
                    return false;
            }
            if (IPAddress.TryParse(host, out IPAddress literal) && literal.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = literal;
                return true;
            }

            try
            {
                foreach (IPAddress candidate in Dns.GetHostEntry(host).AddressList)
                {
                    if (candidate.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = candidate;
                        return true;
                    }
                }
            }
            catch
            {
                // ignored - method will return false so we can show a friendly error.
            }

            return false;
        }

        private void DisplayAttachment(string msg)
        {
            if (exit)
            {
                return;
            }

            chatPanel.Invoke((MethodInvoker)delegate
            {
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
                    chatPanel.ScrollControlIntoView(pic);
                    return;
                }

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
                        Font = new Font("Segoe UI", 9.75f, FontStyle.Underline),
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
                    chatPanel.ScrollControlIntoView(fileContainer);
                }
            });
        }

        private void AddSenderLabel(string user)
        {
            Label userLabel = new Label
            {
                Text = $"[{DateTime.Now:HH:mm}] {user}:",
                AutoSize = true,
                Font = new Font("Segoe UI", 9.75f, FontStyle.Regular)
            };
            chatPanel.Controls.Add(userLabel);
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
        //phóng to ảnh
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

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendServerMessage();
        }

        // Gửi danh sách client cho mọi người
        // Format: [USERLIST]|id1:username1|id2:username2|...
        private void BroadcastUserList()
        {
            try
            {
                List<string> items = new List<string>();
                foreach (var kvp in clients)
                {
                    items.Add($"{kvp.Key}:{kvp.Value.username}"); // username là StringBuilder
                }

                string msg = "[USERLIST]|" + string.Join("|", items);
                // Gửi cho TẤT CẢ (id = -1)
                Send(msg);
            }
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
        }

        private void HandlePrivateMessage(string rawMsg, MyClient sender)
        {
            try
            {
                string[] parts = rawMsg.Split(new[] { '|' }, 3);
                if (parts.Length < 3) return;

                if (!long.TryParse(parts[1], out long targetId)) return;
                string payload = parts[2]; // ví dụ: "Alice (private): Hello"

                Log($"(RIÊNG) {payload}");

                if (clients.TryGetValue(targetId, out MyClient target))
                {
                    // gửi riêng cho 1 client
                    Send(payload, target);
                }
            }
            catch (Exception ex)
            {
                Log(ErrorMsg(ex.Message));
            }
        }
        //Form gửi tin nhắn riêng
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
