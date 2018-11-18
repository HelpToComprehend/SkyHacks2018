# SkyHacks2018
HELP TO COMREHEND

Steps of running the text-to-speech process:
1. Run "Wave Cutter"
	1.1. It is required that there is a folder named "Old" in the the path directory of the program. The folder should contain all of the .wav files to cut.
	1.2. It is also required that there is a folder named "New" in the path directory of the program. The folder swill be used to save the cut .wav files.
	1.3. It is necessary to place the "2h1-2b-test-speakers-and-times.csv" file in the path directory of the program.
	1.4. It may be necessary to provide the program ffmpeg.exe in the path directory of the program.
	
2. Run "Data Transmission"
	2.1. It is required that there is a folder named "SoundSamples" in the directory of the .exe file. The folder should contain all of the pre-cut .wav files to convert obtanied in (1).
	2.2. It is advised that one more folder named "RecognizedSpeech" is created in the same directory. The program will save the output transcription files in that folder. If such a folder is not created beforehand, the program should automatically create it while running.

3. Run "Edit CSV"
	3.1. It is required that there is a folder named "Transcripts" in the directory of the .exe file. The folder should contain all of the transcriptions obtained in (2).
	3.2. The program takes 2 parameters: path to the .csv file with the information about speakers; path to where the output .csv file will be saved


Text Processing is only needed If there is a need to format transcript correctly in a way shown at the official SkyHacks repo. No need to run it if only .wav samples are passed to azure and transcripts are taken from azure projects folder.