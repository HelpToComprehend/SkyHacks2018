using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AzureDataTransmission
{
    class Program
    {
        /// <summary>
        /// Speech-to-text dla jednego pliku.
        /// </summary>
        /// <param name="subKey">Klucz subskrypcji Azure</param>
        /// <param name="serviceRegion">Nasz region (chyba Northern Europe)</param>
        /// <param name="filePath">Ścieżka (chyba może być względna) do pliku WAV</param>
        /// <param name="channelCount">Liczba kanałów (rozmówców)</param>
        /// <returns>Tak szczerze to nie wiem...</returns>
        private static async Task DoTask(String subKey, String serviceRegion, String filePath, String destFilePath)
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription(subKey, serviceRegion);


            var stopRecognition = new TaskCompletionSource<int>();

            // Creates a speech recognizer using file as audio input.
            // Replace with your own audio file name.
            using (var audioInput = AudioConfig.FromWavFileInput(filePath))
            {
                using (var recognizer = new SpeechRecognizer(config, audioInput))
                {
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        var offset = e.Result.OffsetInTicks;
                        Console.WriteLine(e.Result.Duration);
                        Console.WriteLine(e.Result.OffsetInTicks);
                        Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");

                            //if(File.Exists(destFilePath))
                            //File.Delete(destFilePath);

                            File.AppendAllText(destFilePath, e.Result.Text + Environment.NewLine);

                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        Console.WriteLine("\n    Session started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        Console.WriteLine("\n    Session stopped event.");
                        Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }
        }

        static  void Main(string[] args)
        {
            if(!Directory.Exists("SoundSamples"))
            {
                Directory.CreateDirectory("SoundSamples");
                Console.WriteLine("Created SoundSamples folder! Add data to it first. Finishing....");
                if (!Directory.Exists("RecognizedSpeech"))
                {
                    Directory.CreateDirectory("RecognizedSpeech");
                    Console.WriteLine("Created RecognizedSpeech folder! Finishing...");
                   
                }
                Console.ReadKey();
                Environment.Exit(-1);
            }

            if (!Directory.Exists("RecognizedSpeech"))
            {
                Directory.CreateDirectory("RecognizedSpeech");
                Console.WriteLine("Created RecognizedSpeech folder!");
            }
            // Program program = new Program();
            var soundSamples = Directory.GetFiles("SoundSamples", "*.wav");

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            foreach(var file in soundSamples)
            {
                DoTask("f1344ffced734c579b4128a09c88ad5e", "northeurope", file, "RecognizedSpeech\\" + System.IO.Path.GetFileNameWithoutExtension(file) + "recognized.txt" ).Wait();

            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed in seconds: " + stopwatch.ElapsedMilliseconds / 1000);
            Console.ReadKey();
        }
    }
}
