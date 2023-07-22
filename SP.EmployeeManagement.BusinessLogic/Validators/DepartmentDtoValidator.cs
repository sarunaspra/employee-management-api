using FluentValidation;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Validators
{
    public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
    {
        public DepartmentDtoValidator()
        {
            RuleFor(department => department.Name).NotEmpty().MaximumLength(80);
            RuleFor(department => department.Description).NotEmpty().MaximumLength(200);
        }
    }
}
