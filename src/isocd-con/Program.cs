using System;
using System.IO;
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
            Console.CursorVisible = true;

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
                    Console.WriteLine("\nCould not find valid trademark files. Would you like to download them?");
                    var key = Console.ReadKey(true);

                    if(key.Key == ConsoleKey.Y) {
                        Console.WriteLine("\nDownloading trademark files, please wait...");
                        builderSetupResult.HaveTmFiles = TmFileHelper.DownloadTmFiles();
                    }

                    if(!builderSetupResult.HaveTmFiles) {
                        if(key.Key == ConsoleKey.Y) {
                            Console.WriteLine("\nCould not download trademark files - aborting!");
                        }

                        Environment.Exit(-1);
                    }
                }

                if(string.IsNullOrWhiteSpace(options.TrademarkFile) && options.Trademark) {
                    Console.WriteLine("\nYou have not provided a trademark file, so defaulting to CD32");
                    options.TrademarkFile = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, isocd_builder_constants.CD32_TRADEMARK_FILE);
                }

                Console.CursorVisible = false;

                display.Initialise(versionString);
                worker = new BuildIsoWorker();
                worker.WorkerUpdateEvent += WorkerUpdate;
                worker.WorkerCompletedEvent += WorkerCompleted;
                worker.StartWork(options);

                Console.CursorVisible = true;

                return 0;
            }
            catch(Exception ex) {
                Console.WriteLine($"\n{ex.Message}");

                return -1;
            }
            finally {
                Console.CursorVisible = true;
            }
        }

        static void ShowHelp() {
            Console.WriteLine(versionString);
            Console.Write($"Usage: isocd-con");

            var options = new ExtendedOptions();
            Console.Write(options.ToString());
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
