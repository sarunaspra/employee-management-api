using FluentValidation;
using SP.EmployeeManagement.Dto.Dtos;
using System.Text.RegularExpressions;

namespace SP.EmployeeManagement.BusinessLogic.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(employee => employee.FirstName).NotEmpty().Length(2,50);
            RuleFor(employee => employee.LastName).NotEmpty().Length(2,50);
            RuleFor(employee => employee.Email).NotEmpty().EmailAddress();
            RuleFor(employee => employee.PhoneNumber).NotEmpty().Must(BeValidPhoneNumber).WithMessage("Invalid phone number");
            RuleFor(employee => employee.DepartmentId).NotEmpty();
            RuleFor(employee => employee.PositionId).NotEmpty();
            RuleFor(employee => employee.Salary).NotEmpty();
        }

        private bool BeValidPhoneNumber(string phoneNumber) => phoneNumber is not null && Regex.IsMatch(phoneNumber, @"^\+370\d{8}$");
    }
}
