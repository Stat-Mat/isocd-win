using System.Collections.Generic;
using System.Linq;

namespace isocd_builder {
    public static class ValidatableExtensions {
        public static bool IsValid(this IValidatable input) {
            // Sometimes all you care about is a boolean
            return input.ValidationResult().Success;
        }

        public static string ValidationMessage(this IValidatable input) {
            // Other times you just want the message. This is much more rare.
            return input.ValidationResult().Message;
        }

        public static ValidationResult ValidationResult(this IValidatable input) {
            // This avoids needing a null check in our code when we validate nullable objects
            return input == null ? new ValidationResult(false, isocd_builder_constants.VALIDATION_NULL_INPUT_IS_INVALID_MESSAGE, 1) : input.Validate();
        }

        public static ValidationResult ToValidationResult(this List<string> input) {
            // Centralizes boilerplate needed to convert the list of errors into a single string.
            if(input == null) {
                return new ValidationResult(false, isocd_builder_constants.VALIDATION_NULL_INPUT_IS_INVALID_MESSAGE, 1);
            }

            // Enumerate the errors
            var errors = input.ToList();
            var success = !errors.Any();
            var message = success ? isocd_builder_constants.VALIDATION_SUCCESSFUL_MESSAGE : string.Join("\r\n", errors);

            return new ValidationResult(success, message, errors.Count());
        }
    }
}
