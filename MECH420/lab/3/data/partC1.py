import os 
import matplotlib.pyplot as plt
import numpy as np
dir_path = os.path.dirname(os.path.realpath(__file__))


def tachVToVel(v):
    return (v - 0.0024645)/0.00426465

def accelVToA(v):
    return (v - 2.65)*9810*.96

names = ['lab3_C_2.csv', 'lab3_C_1.csv']
for name in names:
    t = []
    tachA = []
    accelA = []
    with open(dir_path+'\\'+name) as f:
        next(f) # flush header line
        init = next(f)
        items = init.split(',')
        prevTach = tachVToVel(float(items[4]))
        for line in f:
            items = line.split(',')
            if len(items) > 1:
                t.append(float(items[0]))
                curTach = 0
                if items[4].rstrip(): # some values are skipped for no reason
                    curTach = tachVToVel(float(items[4]))
                tachA.append((curTach - prevTach)/0.005)
                prevTach = curTach
                accelA.append(accelVToA(float(items[3])))

    plt.clf()
    plt.plot(t, tachA, 'b', label='From tachometer', alpha=0.5)
    plt.plot(t, accelA, 'g', label='From accelerometer', alpha=0.5)    
    plt.title('Acceleration (mm/s^2) over time (s)), data from ' + name)
    plt.ylabel('Acceleration (mm/s^2)')
    plt.xlabel('Time (s)')
    plt.legend()
    plt.savefig(dir_path + '\\' + 'partC1_' + name + '.png')
