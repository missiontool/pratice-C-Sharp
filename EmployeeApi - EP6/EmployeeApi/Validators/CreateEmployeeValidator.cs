using EmployeeApi.Dtos;
using FluentValidation;

namespace EmployeeApi.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator() 
        {
            // 1.名字不能為空，長度不超過50
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("名字不能是空的")
                .MaximumLength(50).WithMessage("名字最長為50字");

            // 2.底薪必須大於0
            RuleFor(x => x.BaseSalary)
                .GreaterThan(0).WithMessage("底薪要>0");

            // 3.獎金不能是負數>=0
            RuleFor(x => x.Bonus)
                .GreaterThanOrEqualTo(0).WithMessage("獎金必須>=0");
        }
    }
}
