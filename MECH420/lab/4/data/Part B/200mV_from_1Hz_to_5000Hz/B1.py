import matplotlib.pyplot as plt
import os 
dir_path = os.path.dirname(os.path.realpath(__file__))

def convertI(i):
    return (i/0.2)/10

V = .2 # V p to p
freq = []
Z = []

names = ['1Hz_200mV.xls.csv', '5Hz_200mV.xls.csv', '10Hz_200mV.xls.csv', '20Hz_200mV.xls.csv', '100Hz_200mV.xls.csv', '500Hz_200mV.xls.csv', '1000Hz_200mV.xls.csv', '5000Hz_200mV.xls.csv']
for name in names:
    with open(dir_path+'\\'+name) as f:
        freqStr = name
        fre = float(freqStr.split('Hz')[0])

        minI = 0
        maxI = 0
        next(f) # flush header line
        for line in f:
            items = line.split(',')
            tempI = float(items[5])
            if tempI < minI:
                minI = tempI
            if tempI > maxI:
                maxI = tempI
        p2pI = maxI - minI
        I = convertI(p2pI)
        Z_ = abs(V/I)

        freq.append(fre)
        Z.append(Z_)

plt.clf()
plt.xscale('log')
plt.plot(freq, Z, 'o-')
plt.title('Impedance spectrum')
plt.ylabel('Impedance (ohm)')
plt.xlabel('Frequency (Hz)')
plt.savefig(dir_path + '\\' + 'B1.png')