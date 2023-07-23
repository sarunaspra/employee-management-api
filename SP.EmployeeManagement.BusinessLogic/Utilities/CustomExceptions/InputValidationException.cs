using FluentValidation.Results;

namespace SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions
{
    public class InputValidationException : Exception
    {
        public List<ValidationFailure> Errors { get; }

        public InputValidationException(List<ValidationFailure> errors)
        {
            Errors = errors;
        }
    }
}
