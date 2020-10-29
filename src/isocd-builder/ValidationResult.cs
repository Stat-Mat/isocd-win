namespace isocd_builder {
    public class ValidationResult {
        public ValidationResult(bool success, string message, int numErrors) {
            Success = success;
            Message = message;
            NumErrors = numErrors;
        }

        public bool Success { get; }

        public string Message { get; }

        public int NumErrors { get; }
    }
}
