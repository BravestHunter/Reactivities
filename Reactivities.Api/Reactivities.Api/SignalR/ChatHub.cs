using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Reactivities.Domain.Comments.Commands;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Comments.Queries;

namespace Reactivities.Api.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(CreateCommentDto dto)
        {
            var createCommentResult = await _mediator.Send(new CreateCommentCommand() { Comment = dto });

            if (createCommentResult.IsSuccess)
            {
                var comment = createCommentResult.GetOrThrow();
                await Clients
                    .Group(dto.ActivityId.ToString(CultureInfo.InvariantCulture))
                    .SendAsync("ReceiveComment", comment);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            var activityId = httpContext?.Request.Query["activityId"].ToString();
            if (string.IsNullOrEmpty(activityId))
            {
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var getCommentsResult = await _mediator.Send(
                new GetCommentsListQuery() { ActivityId = long.Parse(activityId, CultureInfo.InvariantCulture) }
            );

            if (getCommentsResult.IsSuccess)
            {
                var comments = getCommentsResult.GetOrThrow();
                await Clients.Caller.SendAsync("LoadComments", comments);
            }
        }
    }
}
