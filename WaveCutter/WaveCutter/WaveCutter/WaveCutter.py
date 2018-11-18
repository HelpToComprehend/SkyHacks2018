from pydub import AudioSegment
import os
import csv

def convertWAV(dir, newDir, name, index, start, end):
    oldAudio = AudioSegment.from_wav(dir + '/' + name)

    newAudio = oldAudio[float(start)*1000:float(end)*1000]
    newName = os.path.splitext(name)[0]
    newAudio.export(newDir + '/' + newName + str(index) + '.wav', format="wav")

def readFile(fileName):
    rows = []
    with open(fileName) as csvfile:
        reader = csv.reader(csvfile, delimiter=',')
        for row in reader:
            rows.append(row)
    return rows

dirName = 'Old' 
timings = readFile('2h1-2b-test-speakers-and-times.csv')

index = 0
for item in timings:
    convertWAV(dirName, 'New', item[0], index, item[1], item[2]) 
    index = index+1