import datetime
from os import listdir
from PyPDF2 import PdfFileMerger

merger = PdfFileMerger()

d = listdir()
a = [x for x in d if '.pdf' in x]
d = [x[:-4] for x in d if '.pdf' in x and not('result' in x)]
s = list(set([x[:10] for x in d]))
s.sort(key=lambda x: datetime.datetime.strptime(x, '%d-%m-%Y'))
for date in s:
    files = [x for x in a if date in x]
    for f in files:
        merger.append(f)

merger.write("result.pdf")
merger.close()
