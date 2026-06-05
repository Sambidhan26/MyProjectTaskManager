using FluentValidation;
using TaskManager.API.DTOs;

namespace TaskManager.API.Validators
{
    public class CreateTaskItemDtoValidator: AbstractValidator<CreateTaskItemDto>
    {
        public CreateTaskItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

             RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Due date must be in the future.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be a positive integer.");
            RuleFor(x => x.PriorityId)
            .GreaterThan(0)
            .When(x => x.PriorityId.HasValue);
        }
    }
}
