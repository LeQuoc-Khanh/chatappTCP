import socket
import threading
import tkinter as tk
from tkinter import scrolledtext, messagebox

HOST = '127.0.0.1'
PORT = 5000

client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((HOST, PORT))

def receive_messages():
    while True:
        try:
            msg = client.recv(1024).decode('utf-8')
            chat_box.config(state='normal')
            chat_box.insert(tk.END, msg + "\n")
            chat_box.config(state='disabled')
            chat_box.yview(tk.END)
        except:
            messagebox.showerror("Lỗi", "Mất kết nối với server!")
            client.close()
            break

def send_message():
    msg = entry.get()
    entry.delete(0, tk.END)
    if msg:
        client.send(msg.encode('utf-8'))

root = tk.Tk()
root.title("TCP Chat GUI")

chat_box = scrolledtext.ScrolledText(root, width=50, height=20, state='disabled')
chat_box.pack(padx=10, pady=10)

entry = tk.Entry(root, width=40)
entry.pack(side=tk.LEFT, padx=(10,0), pady=(0,10))

send_btn = tk.Button(root, text="Gửi", command=send_message)
send_btn.pack(side=tk.LEFT, padx=10, pady=(0,10))

threading.Thread(target=receive_messages, daemon=True).start()

root.mainloop()
