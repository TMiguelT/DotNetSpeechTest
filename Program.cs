using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Speech.Recognition;
using System.Text;

namespace SpeechRecognitionTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Setup speech recognition
            var sr = new SpeechRecognitionEngine();
            sr.SetInputToDefaultAudioDevice();
            sr.EndSilenceTimeout = sr.InitialSilenceTimeout = new TimeSpan(0, 0, 0, 0, 100);
            sr.LoadGrammar(new DictationGrammar());

            // Make a stopwatch to time the gap between results
            var stopWatch = new Stopwatch();

            // The event handler returns quickly
            sr.SpeechRecognized +=
                (sender, eventArgs) =>
                {
                    Console.WriteLine($"Recognized \"{eventArgs.Result.Text}\" in event handler");
                    stopWatch.Start();
                };

            // The return value is much later
            while (true)
            {
                Console.WriteLine("Listening");
                var result = sr.Recognize();
                stopWatch.Stop();
                if (result != null)
                    Console.WriteLine($"Recognized \"{result.Text}\" in return value, {stopWatch.ElapsedMilliseconds} ms later");
                stopWatch.Reset();
            }
        }
    }
}