#!/usr/bin/env python
# -*- coding=utf-8 -¡Á-
f = open('data.txt', 'r')
s = f.readline()
a = open('data.txt', 'w')
a.close()
c = open('data.txt', 'r+')
c.write(s)
c.write(s)
