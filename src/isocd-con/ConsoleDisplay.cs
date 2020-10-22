using System;
using isocd_builder;

namespace isocd_con {
    /// <summary>
    /// This class provides functionality for display updates in the console window.
    /// </summary>
    class ConsoleDisplay {
        readonly object updateLock = new object();

        const int BAR_EXTRA_CHARS = 5;

        int _barPos = 3;
        int _percentPos;
        int _lastWidth;
        int _lastProgress;
        bool _ending;
        readonly int _maximumWidth;
        readonly int _actualWidth;

        public ConsoleDisplay(int maximumWidth) {
            _maximumWidth = maximumWidth;
            _actualWidth = maximumWidth + BAR_EXTRA_CHARS;
        }

        public void Initialise(string versionString) {
            Console.WriteLine("");
            WriteCentre(versionString);
            Console.WriteLine("\r\n");

            PaddedText(" ╔",  '═',  "╗", _maximumWidth + 2);
            PaddedText(" ║ ", '▒', " ║ 0%", _maximumWidth);
            PaddedText(" ╚",  '═',  "╝", _maximumWidth + 2);

            Console.WriteLine("");
            WriteCentre("Scanning directories...");

            Console.CursorTop -= 3;
            _percentPos = _maximumWidth + BAR_EXTRA_CHARS + 1;
        }

        public void FinalMessage(string message) {
            // Only allow one thread at a time to update the display
            lock(updateLock) {
                Console.CursorTop += 3;
                WriteCentre(message);
                Console.WriteLine("");
                _ending = true;
            }
        }

        public void UpdateProgressBar(WorkerUpdateStatus workerUpdateStatus) {
            // Only allow one thread at a time to update the display
            lock(updateLock) {
                if(!_ending) {
                    Console.CursorLeft = _barPos;
                    var width = (int) Math.Ceiling((double)workerUpdateStatus.Progress / 100 * _maximumWidth);

                    if(width > _lastWidth) {
                        var blocksToDraw = width - _lastWidth;
                        Console.Write(string.Empty.PadLeft(blocksToDraw, '█'));
                        _lastWidth = width;
                        _barPos += blocksToDraw;
                    }

                    Console.CursorTop += 3;
                    WriteCentre($"Processing entry {workerUpdateStatus.CurrentEntry} of {workerUpdateStatus.TotalEntries}");
                    Console.CursorTop -= 3;

                    if(workerUpdateStatus.Progress > _lastProgress) {
                        Console.CursorLeft = _percentPos;
                        Console.Write($"{workerUpdateStatus.Progress}%");
                        _lastProgress = workerUpdateStatus.Progress;
                    }
                }
            }
        }

        void PaddedText(string start, char middle, string end, int maxWidth) {
            var output = $"{start}{string.Empty.PadLeft(maxWidth, middle)}{end}";
            Console.WriteLine(output);
        }

        void WriteCentre(string value) {
            var padding = (_actualWidth - value.Length) / 2;

            // String is longer than _maximumWidth, so just set zero padding
            if(padding < 0) {
                padding = 0;
            }

            Console.Write($"\r{string.Empty.PadLeft(padding, ' ')}{value}{string.Empty.PadRight(padding, ' ')}");
        }
    }
}
