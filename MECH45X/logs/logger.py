from datetime import datetime
import tkinter as tk
import json
import os

# init root window
root = tk.Tk()
root.wm_title("Logger")
root.geometry("600x650")

# init time stuff
date_time = tk.Frame(master=root)
date_time.pack(fill="both", expand=True)
date_time.grid_columnconfigure(0, weight=1)
date_time.grid_columnconfigure(1, weight=1)
date_time.grid_columnconfigure(2, weight=1)

def SetStartTime():
    startTime.delete('1.0', "end-1c")
    startTime.insert("end-1c", datetime.now().strftime("%H:%M:%S"))
def SetEndTime():
    endTime.delete('1.0', "end-1c")
    endTime.insert("end-1c", datetime.now().strftime("%H:%M:%S"))

dateLabel = tk.Label(master=date_time, text="Date:")
dateLabel.grid(row=0, column=0)
date = tk.Text(master=date_time, height=1)
date.insert("end-1c", datetime.now().strftime("%d-%m-%Y"))
date.grid(row=1, column=0)
startLabel = tk.Button(master=date_time, text="Set start time:", command=SetStartTime)
startLabel.grid(row=0, column=1)
startTime = tk.Text(master=date_time, height=1)
SetStartTime()
startTime.grid(row=1, column=1)
endLabel = tk.Button(master=date_time, text="Set end time:", command=SetEndTime)
endLabel.grid(row=0, column=2)
endTime = tk.Text(master=date_time, height=1)
SetEndTime()
endTime.grid(row=1, column=2)

# init info stuff
info = tk.Frame(master=root)
info.pack(fill="both", expand=True)
info.grid_columnconfigure(0, weight=1)
info.grid_columnconfigure(1, weight=1)

optionList = ("Team meeting", "Supervisor meeting", "Alone work time", "Sponsor meeting")
selectedMeetingType = tk.StringVar()
selectedMeetingType.set(optionList[0])
meetingType = tk.OptionMenu(info, selectedMeetingType, *optionList)
meetingType.grid(row=0, column=0)

def GetAttendance():
    ret = []
    if friend:
        ret.append('Ratthamnoon Prakitpong')
    if gianni:
        ret.append('Gianni Co')
    if hassan:
        ret.append('Hassan Iqbal')
    if kevin:
        ret.append('Kevin Yang')
    return ret

attendance = tk.Frame(master=info)
attendance.grid(row=0, column=1)
attendanceLabel = tk.Label(master=attendance, text="Attendance:")
attendanceLabel.pack()
friend = tk.BooleanVar()
friend.set(True)
friendCheck = tk.Checkbutton(master=attendance, text="Friend", variable=friend)
friendCheck.pack()
gianni = tk.BooleanVar()
gianni.set(True)
gianniCheck = tk.Checkbutton(master=attendance, text="Gianni", variable=gianni)
gianniCheck.pack()
hassan = tk.BooleanVar()
hassan.set(True)
hassanCheck = tk.Checkbutton(master=attendance, text="Hassan", variable=hassan)
hassanCheck.pack()
kevin = tk.BooleanVar()
kevin.set(True)
kevinCheck = tk.Checkbutton(master=attendance, text="Kevin", variable=kevin)
kevinCheck.pack()

# init note writing box
notesLabel = tk.Label(master=root, text="Notes:")
notesLabel.pack()
notes = tk.Text(master=root)
notes.pack()

# init write to file stuff
def WriteToFile():
    if endTime.get("1.0",'end-1c') == "":
        SetEndTime()
    data = {
        "date" : date.get("1.0",'end-1c'),
        "startTime" : startTime.get("1.0",'end-1c'),
        "endTime" : endTime.get("1.0",'end-1c'),
        "meetingType" : selectedMeetingType.get(),
        "attendance" : GetAttendance(),
        "notes" : list(notes.get("1.0", 'end-1c').split("\n")) 
    }
    path = os.path.dirname(os.path.realpath(__file__))
    name = date.get("1.0",'end-1c')
    dir = os.path.join(path, name+'.json')
    i = 0
    while (os.path.isfile(dir)):
        dir = os.path.join(path, name+'--'+str(i)+'.json')
        i = i+1
    print(dir)
    with open(dir, 'w') as fp:
        json.dump(data, fp, indent=4)
def SetEndAndWriteToFile():
    SetEndTime()
    WriteToFile()

writeBtn = tk.Button(master=root, text="Write to file", command=WriteToFile)
writeBtn.pack()
setEndAndWriteBtn = tk.Button(master=root, text="Set end time and write to file", command=SetEndAndWriteToFile)
setEndAndWriteBtn.pack()

# on closing, write to file if no written yet
def on_closing():
    SetEndAndWriteToFile()
    root.destroy()
root.protocol("WM_DELETE_WINDOW", on_closing)

tk.mainloop()