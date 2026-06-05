using FluentValidation;
using TaskManager.API.DTOs;

namespace TaskManager.API.Validators
{
    public class CreateCommentDtoValidator:AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(300).WithMessage("Content cannot exceed 300 characters.");

        }
    }
}
