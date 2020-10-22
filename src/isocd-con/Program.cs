using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using isocd_builder;

namespace isocd_con {
    class Program {
        static readonly ConsoleDisplay display = new ConsoleDisplay(42);
        static BuildIsoWorker worker;

        static readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;
        static readonly string versionString = $"ISOCD-Con v{version.Major}.{version.Minor} - Ben Squibb 2020";

        static ExtendedOptions options;

        static int Main(string[] args) {
            Console.CursorVisible = false;

            // Setup a delegate to handle the user killing the app with CTRL+C
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                display.FinalMessage("Aborting...");
                if(worker != null) {
                    Console.CursorTop -= 4;
                    worker.StopWork();
                }
            };

            try {
                // Parse the command line arguments
                var arguments = new InputArguments(args);
                options = arguments.ToObject<ExtendedOptions>();

                if(args.Length == 0 || options == null) {
                    ShowHelp();
                    Environment.Exit(0);
                }

                if(!options.IsValid()) {
                    Console.WriteLine(options.ValidationResult().Message);
                    Environment.Exit(-1);
                }

                // Make sure everything is setup ready to use the isocd-win-builder library
                var builderSetupResult = BuilderSetupHelper.Setup();

                if(!builderSetupResult.HaveTmFiles) {
                    Console.WriteLine("\r\nCould not find valid trademark files. Would you like to download them?");
                    var key = Console.ReadKey(true);

                    if(key.Key == ConsoleKey.Y) {
                        Console.WriteLine("\r\nDownloading trademark files, please wait...");
                        builderSetupResult.HaveTmFiles = TmFileHelper.DownloadTmFiles();
                    }

                    if(!builderSetupResult.HaveTmFiles) {
                        if(key.Key == ConsoleKey.Y) {
                            Console.WriteLine("\r\nCould not download trademark files - aborting!");
                        }

                        Environment.Exit(-1);
                    }
                }

                if(string.IsNullOrWhiteSpace(options.TrademarkFile) && options.Trademark) {
                    Console.WriteLine("\r\nYou have not provided a trademark file, so defaulting to CD32");
                    options.TrademarkFile = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, isocd_builder_constants.CD32_TRADEMARK_FILE);
                }

                display.Initialise(versionString);
                worker = new BuildIsoWorker();
                worker.WorkerUpdateEvent += WorkerUpdate;
                worker.WorkerCompletedEvent += WorkerCompleted;
                worker.StartWork(options);

                Console.CursorVisible = true;

                return 0;
            }
            catch(Exception ex) {
                Console.WriteLine("The following error occurred:\r\n\r\n");
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        static void ShowHelp() {
            // Get a dictionary of all the CmdLineOptionAttributes with the property name as key
            var attributeDictionary = typeof(ExtendedOptions)
            .GetProperties()
            .Where(p => p.GetCustomAttribute<CmdLineOptionAttribute>() != null)
            .Select(
                p =>
                new KeyValuePair<string, CmdLineOptionAttribute>(
                    p.Name,
                    p.GetCustomAttribute<CmdLineOptionAttribute>()
                )
            )
            .OrderBy(p => p.Value.Id)
            .ToDictionary(p => p.Key, p => p.Value);

            var lenLongestName = 0;
            var lenLongestShortName = 0;

            foreach(var item in attributeDictionary) {
                if(item.Key.Length + item.Value.ParamName?.Length > lenLongestName) {
                    lenLongestName = (item.Key.Length + item.Value.ParamName?.Length).Value;
                }

                if(item.Value.ShortName.Length + item.Value.ParamName?.Length > lenLongestShortName) {
                    lenLongestShortName = (item.Value.ShortName.Length + item.Value.ParamName?.Length).Value;
                }
            }

            Console.WriteLine(versionString);
            Console.Write($"Usage: isocd-con");

            foreach(var item in attributeDictionary.Where(a => a.Value.IsRequired == true)) {
                Console.Write($" -{item.Key.ToLower()} {item.Value.ParamName}");
            }

            Console.WriteLine(" [MORE OPTIONS]");
            Console.WriteLine($"Full options list:");

            foreach(var item in attributeDictionary) {
                var param = !string.IsNullOrWhiteSpace(item.Value.ParamName) ? $" {item.Value.ParamName}" : "";
                var longPad = (lenLongestName - (item.Key.Length + param.Length - 1));
                var shortPad = (lenLongestShortName - (item.Value.ShortName.Length + param.Length - 1));

                Console.WriteLine($"  -{item.Value.ShortName}{param},{string.Empty.PadLeft(shortPad, ' ')} -{item.Key.ToLower()}{param} {string.Empty.PadLeft(longPad, ' ')}{item.Value.Description}");
            }
        }

        static void WorkerUpdate(object sender, WorkerUpdateEventArgs e) {
            display.UpdateProgressBar(e.State);
        }

        static void WorkerCompleted(object sender, WorkerCompletedEventArgs e) {
            switch(e.Status) {
                case WorkerCompletedStatus.Success:
                    display.FinalMessage("Done!");
                    break;

                case WorkerCompletedStatus.Error:
                    display.FinalMessage(e.Exception.Message);
                    break;

                case WorkerCompletedStatus.Cancelled:
                    display.FinalMessage("Aborted!");
                    break;
            }
        }
    }
}
