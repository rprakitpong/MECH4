import os 
dir_path = os.path.dirname(os.path.realpath(__file__))
print(dir_path)

files = ['e1_motor_0.5V.csv', 'e1_motor_0.7V.csv', 'e1_motor_0.9V.csv']

for name in files:
    with open(dir_path+'\\'+name) as f:
        counts = []
        counter = 0
        prev = ''
        for line in f:
            items = line.split(',')
            if len(items) > 1:
                if items[1] == '1' and prev == '':
                    counts.append(counter)
                    counter = 0
                counter = counter + 1
                prev = items[1]
        print(name)
        print(sum(counts[1:len(counts)])/(len(counts)-1))