import socket
import threading

HOST = '127.0.0.1'
PORT = 5000

client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((HOST, PORT))

def receive():
    while True:
        try:
            msg = client.recv(1024).decode('utf-8')
            print(msg)
        except:
            print("Ngắt kết nối!")
            client.close()
            break

def write():
    while True:
        msg = input("")
        client.send(msg.encode('utf-8'))

threading.Thread(target=receive).start()
threading.Thread(target=write).start()
