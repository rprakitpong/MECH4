import json
from os import listdir
import re
import pathlib
from io import BytesIO
from pdfdocument.document import PDFDocument

files = listdir()
for file in files:
    if '.json' in file:
        with open(file) as fi:
            data = json.load(fi)
            f = BytesIO()
            pdf = PDFDocument(f)
            pdf.init_report()
            for title in data:
                titleFormatted = re.sub(r'(?<!^)(?=[A-Z])', ' ', title).capitalize()
                pdf.h1(titleFormatted)
                if type(data[title]) == type('str'):
                    pdf.p(data[title])
                else:
                    for line in data[title]:
                        if line[:2] != '- ':
                            line = '- '+line
                        pdf.p(line)
            pdf.generate()
            pathlib.Path(file[:-5]+'.pdf').write_bytes(f.getvalue())
            #return f.getvalue()
                