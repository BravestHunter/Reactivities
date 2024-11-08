using AutoMapper;
using FluentValidation;
using MediatR;
using Reactivities.Application.Core;
using Reactivities.Domain.Models;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result>
        {
            public Activity Activity { get; set; }
        }

        internal class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        internal class Handler : IRequestHandler<Command, Result>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var existingActivity = await _dataContext.Activities.FindAsync(request.Activity.Id);
                    if (existingActivity == null)
                    {
                        return Result.Failure("Failed to find activity");
                    }

                    _mapper.Map(request.Activity, existingActivity);

                    var result = await _dataContext.SaveChangesAsync() > 0;
                    if (!result)
                    {
                        return Result.Failure("Failed to update activity");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to update activity", ex);
                }
            }
        }
    }
}
