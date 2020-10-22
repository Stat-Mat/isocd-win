namespace isocd_builder {
    public class ValidationResult {
        public ValidationResult(bool success, string message) {
            Success = success;
            Message = message;
        }
        public bool Success { get; }

        public string Message { get; }
    }
}
