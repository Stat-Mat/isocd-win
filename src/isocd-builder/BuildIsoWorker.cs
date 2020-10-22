using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace isocd_builder {
    /// <summary>
    /// This class provides a wrapper around the build ISO functionality to allow for cross-threaded and aysnchronous operation.
    /// </summary>
    public class BuildIsoWorker {
        CancellationTokenSource cts;

        public CancellationToken Token {
            get {
                return cts.Token;
            }
        }

        public delegate void WorkerUpdate(object source, WorkerUpdateEventArgs e);
        public delegate void WorkerCompleted(object source, WorkerCompletedEventArgs e);

        readonly SendOrPostCallback progressReporter;
        readonly SendOrPostCallback completedReporter;
        AsyncOperation asyncOperation;

		WorkerUpdate workerUpdateEvent;
		
        public event WorkerUpdate WorkerUpdateEvent {
            add {
                workerUpdateEvent += value;
            }
            remove {
                workerUpdateEvent -= value;
            }
        }

        WorkerCompleted workerCompletedEvent;

        public event WorkerCompleted WorkerCompletedEvent {
            add {
                workerCompletedEvent += value;
            }
            remove {
                workerCompletedEvent -= value;
            }
        }
		
        public BuildIsoWorker() {
            progressReporter = new SendOrPostCallback(ProgressReporter);
            completedReporter = new SendOrPostCallback(CompletedReporter);
            cts = new CancellationTokenSource();
        }

        void ProgressReporter(object args) {
            if(workerUpdateEvent != null) {
                workerUpdateEvent.Invoke(this, (WorkerUpdateEventArgs)args);
            }
        }

        void CompletedReporter(object args) {
            if(workerCompletedEvent != null) {
                workerCompletedEvent.Invoke(this, (WorkerCompletedEventArgs)args);
            }
        }

        public async Task StartWorkAsync(Options options) {
            asyncOperation = AsyncOperationManager.CreateOperation(null);

            await Task.Run(() =>
                StartWork(options)
            );
        }

        public void StartWork(Options options) {
            var iso9660 = new Iso9660(options);
            var workerCompletedEventArgs = new WorkerCompletedEventArgs();          

            try {
                iso9660.BuildIso(this);
                workerCompletedEventArgs.Status = WorkerCompletedStatus.Success;
            }
            catch(OperationCanceledException) {
                workerCompletedEventArgs.Status = WorkerCompletedStatus.Cancelled;         
                File.Delete(options.OutputFile);
            }
            catch(Exception ex) {
                workerCompletedEventArgs.Status = WorkerCompletedStatus.Error;
                workerCompletedEventArgs.Exception = ex;
            }
            finally {
                if(asyncOperation != null) {
                    // This call will be on the callers UI thread to ensure that updating the UI
                    // (e.g. progress bar etc) will not trigger an invalid cross-thread operation exception
                    asyncOperation.Post(completedReporter, workerCompletedEventArgs);
                }
                else {
                    CompletedReporter(workerCompletedEventArgs);
                }
            }
        }

        public void StopWork() {
            if(cts != null) {
                cts.Cancel();
            }
        }

        public void ReportProgress(WorkerUpdateStatus reportProgressState) {
            reportProgressState.Progress = (int)((double)reportProgressState.CurrentEntry / reportProgressState.TotalEntries * 100.00);
		
            var workerUpdatedEventArgs = new WorkerUpdateEventArgs {
                State = reportProgressState
            };

            if(asyncOperation != null) {
                // This call will be on the callers UI thread to ensure that updating the UI
                // (e.g. progress bar etc) will not trigger an invalid cross-thread operation exception
                asyncOperation.Post(progressReporter, workerUpdatedEventArgs);
            }
            else {
                ProgressReporter(workerUpdatedEventArgs);
            }
        }
    }
}
