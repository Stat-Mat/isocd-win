using System;

namespace isocd_builder {
    public class WorkerCompletedEventArgs : EventArgs {
        public WorkerCompletedStatus Status;
        public Exception Exception;
    }
}
