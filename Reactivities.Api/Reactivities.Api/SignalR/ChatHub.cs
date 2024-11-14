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
            var comment = await _mediator.Send(new CreateCommentCommand() { Comment = dto });

            await Clients
                .Group(dto.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var result = await _mediator.Send(new GetCommentsListQuery() { ActivityId = long.Parse(activityId) });

            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
