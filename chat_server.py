import socket
import threading

HOST = '127.0.0.1'  # localhost
PORT = 5000

clients = []  # Danh sách client đang kết nối

def handle_client(conn, addr):
    print(f"[Kết nối mới] {addr}")
    while True:
        try:
            msg = conn.recv(1024).decode('utf-8')
            if not msg:
                break
            print(f"{addr}: {msg}")
            broadcast(f"{addr}: {msg}", conn)
        except:
            break
    conn.close()
    clients.remove(conn)
    print(f"[Ngắt kết nối] {addr}")

def broadcast(message, sender_conn):
    for client in clients:
        if client != sender_conn:
            client.send(message.encode('utf-8'))

def start_server():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen()
    print(f"[SERVER] Đang lắng nghe tại {HOST}:{PORT}")

    while True:
        conn, addr = server.accept()
        clients.append(conn)
        thread = threading.Thread(target=handle_client, args=(conn, addr))
        thread.start()

if __name__ == "__main__":
    start_server()
