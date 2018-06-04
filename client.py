import socket
import time

s = socket.socket()
host = '127.0.0.1'
port = 8820

s.connect((host, port))

s.send(bytes(str('20005assaf06123456').encode('utf-8')))
s.send(bytes(str('225').encode('utf-8')))
#s.send(bytes(str('21305room351010').encode('utf-8')))
#s.send(bytes(str('217').encode('utf-8')))
#s.send(bytes(str('21915').encode('utf-8')))
#s.send(bytes(str('21915').encode('utf-8')))

print(s.recv(100))
print(s.recv(100))

s.close