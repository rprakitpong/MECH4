import os 
import matplotlib.pyplot as plt
import numpy as np
dir_path = os.path.dirname(os.path.realpath(__file__))
print(dir_path)
name = 'e3_50RPM.csv' 

def file_len(fname):
    with open(dir_path+'\\'+fname) as f:
        for i, l in enumerate(f):
            pass
    return i + 1

filelen = file_len(name)
t = []
rpm = []
cell0 = []
cell1 = []
with open(dir_path+'\\'+name) as f:
    next(f) # discard first line
    counter = 0 # to flush out initial data
    startSaving = False
    initTime = 0
    for line in f:
        items = line.split(',')

        counter = counter + 1
        if counter > filelen / 2:
            t.append(float(items[0]))
            rpm.append(float(items[1]))
            cell0.append(float(items[2]))
            cell1.append(float(items[3]))

m = -6.174
b = .124
T = [(c0 - c1 - b)/m for c0, c1 in zip(cell0, cell1)]
avgT = np.mean(T)
stdT = np.std(T)

plt.clf()
plt.plot(t, T)
plt.title('Torque (Nm) vs Time (s)\nAverage torque = ' + str(avgT) + ' Nm\nStandard deviation = ' + str(stdT) + ' Nm')
plt.ylabel('Torque (Nm)')
plt.xlabel('Time (s)')
plt.savefig(dir_path + '\\' + 'e3.png')

