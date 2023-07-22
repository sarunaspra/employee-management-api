using FluentValidation;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Validators
{
    public class PositionDtoValidator : AbstractValidator<PositionDto>
    {
        public PositionDtoValidator()
        {
            RuleFor(position => position.Title).NotEmpty().MaximumLength(80);
            RuleFor(position => position.Description).NotEmpty().MaximumLength(200);
        }
    }
}
