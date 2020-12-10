import os 
dir_path = os.path.dirname(os.path.realpath(__file__))
print(dir_path)

files = ['e1_motor_0.5V.csv', 'e1_motor_0.7V.csv', 'e1_motor_0.9V.csv']

for name in files:
    with open(dir_path+'\\'+name) as f:
        counter = 0
        flag = False
        for line in f:
            items = line.split(',')
            if len(items) > 1:
                if items[1] == '1' and flag == False:
                    flag = True
                    counter = counter + 1
                if items[1] == '' and flag == True:
                    flag = False
        print(name)
        print(counter)