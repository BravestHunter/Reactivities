using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, Result>
    {
        private readonly IActivityRepository _repository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public DeleteActivityHandler(IActivityRepository repository, IUserAccessor userAccessor, IMapper mapper)
        {
            _repository = repository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActivity = await _repository.GetById(request.Id);
                if (existingActivity == null)
                {
                    return Result.Failure("Failed to find activity");
                }

                await _repository.Delete(existingActivity);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
