import pandas as pd
import os 
dir_path = os.path.dirname(os.path.realpath(__file__))

xls = []
xlsx = []
for file in os.listdir(dir_path):
    if file.endswith(".xls"):
        xls.append(file)
    if file.endswith('.xlsx'):
        xlsx.append(file)

toConvert = xls + xlsx
for name in toConvert:
    data_xls = pd.read_excel(dir_path+'\\'+name, 'Sheet1', index_col=None)
    data_xls.to_csv(dir_path+'\\'+name+'.csv', encoding='utf-8')