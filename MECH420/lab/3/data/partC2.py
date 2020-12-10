import os 
import matplotlib.pyplot as plt
import numpy as np
from scipy import integrate
dir_path = os.path.dirname(os.path.realpath(__file__))


def tachVToVel(v):
    return (v - 0.0024645)/0.00426465

def accelVToA(v):
    return (v - 2.65)*9810

def potVToX(v):
    return (v - 10.306)/(-.0344)

names = ['lab3_C_2.csv', 'lab3_C_1.csv']
for name in names:
    t = []
    potX = []
    tachV = []
    accelA = []
    with open(dir_path+'\\'+name) as f:
        next(f) # flush header line
        for line in f:
            items = line.split(',')
            if len(items) > 1:
                t.append(float(items[0]))
                potX.append(potVToX(float(items[2])))
                curTach = 0
                if items[4].rstrip(): # some values are skipped for no reason
                    curTach = tachVToVel(float(items[4]))
                tachV.append(curTach)
                accelA.append(accelVToA(float(items[3])))

    tachX = integrate.cumtrapz(tachV, t)
    tachX = [x + potX[0] for x in tachX]

    accelV = integrate.cumtrapz(accelA, t)
    #accelV = [x + tachV[0] for x in accelV]
    accelV = [(accelV[x] + tachV[0])-6100*x/len(accelV) for x in range(len(accelV))] # rescaled
    accelX = integrate.cumtrapz(accelV, t[1:])
    #accelX = [x + potX[0] for x in accelX]
    accelX = [(accelX[x] + potX[0])-800*x/len(accelX) for x in range(len(accelX))] # rescaled

    plt.clf()
    plt.plot(t, potX, 'r', label='From potentiometer', alpha=0.5)
    plt.plot(t[1:], tachX, 'b', label='From tachometer', alpha=0.5)
    plt.plot(t[2:], accelX, 'g', label='From accelerometer', alpha=0.5)    
    plt.title('Position (mm) over time (s)), data from ' + name)
    plt.ylabel('Position (mm)')
    plt.xlabel('Time (s)')
    plt.legend()
    plt.savefig(dir_path + '\\' + 'partC2_rescaled_' + name + '.png')
