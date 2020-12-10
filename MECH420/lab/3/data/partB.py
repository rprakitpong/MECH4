import os 
import matplotlib.pyplot as plt
import numpy as np
dir_path = os.path.dirname(os.path.realpath(__file__))

name = 'lab3_B.csv'
avgVel = []
avgTach = []
highPosThres = 200
lowPosThres = 90
with open(dir_path+'\\'+name) as f:
    next(f) # flush header line
    next(f)
    counting = True
    dir = True # true = up, false = down
    tempVel = []
    tempTach = []
    for line in f:
        items = line.split(',')
        if len(items) > 1:
            curPos = float(items[5])
            if counting:
                if (dir and curPos > highPosThres) or (not dir and curPos < lowPosThres):
                        counting = False
                        avTempVel = sum(tempVel)/len(tempVel)
                        avTempTach = sum(tempTach)/len(tempTach)
                        avgVel.append(avTempVel)
                        avgTach.append(avTempTach)
                        tempVel = []
                        tempTach = []
            else:
                if dir:
                    if curPos < highPosThres:
                        counting = True
                        dir = not dir
                else:
                    if curPos > lowPosThres:
                        counting = True
                        dir = not dir

            if counting:
                tempVel.append(float(items[6]))
                tempTach.append(float(items[4]))

x = avgVel
y = avgTach
m, b = np.polyfit(x, y, 1)

plt.clf()
plt.plot(x, y, 'bo--')    
plt.title('Tachometer voltage (V) vs Velocity (mm/s)\nm=' + str(m) + ',b=' + str(b))
plt.ylabel('Voltage (V)')
plt.xlabel('Velocity (mm/s)')
plt.savefig(dir_path + '\\' + 'partB' + '.png')