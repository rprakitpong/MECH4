import os 
import matplotlib.pyplot as plt
import numpy as np
dir_path = os.path.dirname(os.path.realpath(__file__))
print(dir_path)

def plotter(files_right_, files_left, savename=None):
    t_08 = [] # underscore == negative == left weights
    t_16 = []
    t_24 = []
    t_32 = []
    t00 = []
    t08 = []
    t16 = []
    t24 = []
    t32 = []
    for name in files_right_:
        with open(dir_path+'\\'+name) as f:
            for line in f:
                items = line.split(',')
                if len(items) > 1:
                    if items[1] == '':
                        t00.append(float(items[2]) - float(items[3]))
                    if items[1] == '.08':
                        t08.append(float(items[2]) - float(items[3]))
                    if items[1] == '.16':
                        t16.append(float(items[2]) - float(items[3]))
                    if items[1] == '.24':
                        t24.append(float(items[2]) - float(items[3]))
                    if items[1] == '.32':
                        t32.append(float(items[2]) - float(items[3]))

    for name in files_left:
        with open(dir_path+'\\'+name) as f:
            for line in f:
                items = line.split(',')
                if len(items) > 1:
                    if items[1] == '':
                        t00.append(float(items[2]) - float(items[3]))
                    if items[1] == '.08':
                        t_08.append(float(items[2]) - float(items[3]))
                    if items[1] == '.16':
                        t_16.append(float(items[2]) - float(items[3]))
                    if items[1] == '.24':
                        t_24.append(float(items[2]) - float(items[3]))
                    if items[1] == '.32':
                        t_32.append(float(items[2]) - float(items[3]))

    x = [-0.32, -0.24, -0.16, -0.08, 0, 0.08, 0.16, 0.24, 0.32]
    y = [sum(n)/len(n) for n in [t_32, t_24, t_16, t_08, t00, t08, t16, t24, t32]]
    m, b = np.polyfit(x, y, 1)
    
    if savename:
        plt.clf()
        plt.plot(x, y, 'bo--')    
        plt.title('Output voltage (V) vs Input Torque (Nm)\nm=' + str(m) + ',b=' + str(b))
        plt.ylabel('Output (V)')
        plt.xlabel('Torque (Nm)')
        plt.savefig(dir_path + '\\' + savename + '.png')

    return y

def getHysteresisError(filename):
    firstPass = [[], [], []]
    secondPass = [[], [], []]
    firstPassDone = False
    for name in files_right_:
        with open(dir_path+'\\'+name) as f:
            for line in f:
                items = line.split(',')
                if len(items) > 1:
                    ind = 3
                    if items[1] == '.08':
                        ind = 0
                    if items[1] == '.16':
                        ind = 1
                    if items[1] == '.24':
                        ind = 2
                    if items[1] == '.32':
                        firstPassDone = True
                    
                    if ind < 3:
                        val = float(items[2]) - float(items[3])
                        if firstPassDone:
                            secondPass[ind].append(val)
                        else:
                            firstPass[ind].append(val)
    avgFirstPass = [sum(n)/len(n) for n in firstPass]
    avgSecondPass = [sum(n)/len(n) for n in secondPass]
    #print(sum([abs(x) for x in [(f - s) for f, s in zip(avgFirstPass, avgSecondPass)]])/len(avgFirstPass)) # for part B4
    # take the two avg error, times each by 3, sum them, divide by 9 to get total avg 
    error = [100 * (f - s) / max(f, s) for f, s in zip(avgFirstPass, avgSecondPass)]
    error = [abs(n) for n in error]
    return error

# 1
files_right_ = ['e2_torque_0.csv', 'e2_torque_one_by_one_right.csv', 'e2_torque_one_by_one_right_2.csv']
files_left = ['e2_torque_one_by_one_left.csv', 'e2_torque_one_by_one_left_2.csv']
plotter(files_right_, files_left, savename='e2-1')

#2 
files_right_ = ['e2_torque_one_by_one_right_2.csv']
files_left = ['e2_torque_one_by_one_left_2.csv']
plotter(files_right_, files_left, savename='e2-2')

#3
files_right_ = ['e2_torque_one_by_one_right_2.csv']
files_left = ['e2_torque_one_by_one_left_2.csv']

x = [-0.32, -0.24, -0.16, -0.08, 0, 0.08, 0.16, 0.24, 0.32]
m = -6.174
b = .124
theory = [b + m*n for n in x]
real = plotter(files_right_, files_left)
print(sum([abs(x) for x in [(t - r) for t, r in zip(theory, real)]])/len(theory)) #for part B4
nonlinearError = [100 * (t - r) / t for t, r in zip(theory, real)]
nonlinearError = [abs(a) for a in nonlinearError]

e_right = getHysteresisError(files_right_)
e_left = getHysteresisError(files_left)
e_left.reverse()
hysteresisError = [0] + e_left + [0] + e_right + [0] # 0s represent no hysteresis error at 0 and .32 Nm, where they are end points

plt.clf()
plt.plot(x, nonlinearError, 'bo--', label='Nonlinear error')
plt.plot(x, hysteresisError, 'go--', label='Hysteresis error')    
plt.title('Errors (%) at torque loads (Nm)')
plt.ylabel('Percent error (%)')
plt.xlabel('Torque (Nm)')
plt.legend()
plt.savefig(dir_path + '\\' + 'e2-3.png')
