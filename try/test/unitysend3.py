#!/usr/bin/env python
# -*- coding=utf-8 -¡Á-

import socket
from time import sleep
def server():
    host = '10.22.22.234'
    port = 10010
    s = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    s.bind((host,port))
    s.listen(5)
    while(1):
        sock,addr = s.accept()
        print "got connection from ",sock.getpeername()
        data = sock.recv(1024)
        return data

def send(data):
    host = '10.22.22.153'
    port = 10000
    s = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    s.connect((host,port))
    s.send(data)
    s.close

if __name__=='__main__':
    f = open('data.txt', 'r')
    s = f.readline()
    send(s)
    m = server()
    a = open('data.txt', 'w')
    a.close()
    c = open('data.txt', 'r+')
    c.write(m);
